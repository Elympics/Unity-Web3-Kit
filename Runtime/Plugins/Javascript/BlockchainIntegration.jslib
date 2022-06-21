var blockchainIntegration = {
    $requestHandler: {
        currentResponseTicket: 1,
        responsesMap: {},

        handleRequest: function (promise, responseSerializer) {
            var responseTicket = this.currentResponseTicket++;
            promise
                .then(response => {
                    var serialized = responseSerializer(response);
                    // make sure responseData is not undefined, otherwise the transaction will wait forever
                    if (serialized === undefined)
                    {
                        serialized = '';
                    }
                    this.responsesMap[responseTicket] = serialized;
                })
                .catch((error) => {
                    this.responsesMap[responseTicket] = "ERROR:" + error.code;
                    console.log(error.message);
                });
            return responseTicket;
        },

        getResponseData: function (ticket) {
            if (this.isWaitingForResponse(ticket))
                return undefined;
            var returnStr = this.responsesMap[ticket];
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            return buffer;
        },

        isWaitingForResponse: function (ticket) {
            return this.responsesMap[ticket] === undefined;
        }
    },

    GetResponseData: function (ticket) {
        return requestHandler.getResponseData(ticket);
    },

    IsWaitingForResponse: function (ticket) {
        return requestHandler.isWaitingForResponse(ticket);
    },

    MetaMaskConnect: function () {
        if (window.ethereum) {
            web3 = new Web3(window.ethereum);
            return requestHandler.handleRequest(ethereum.request({ method: 'eth_requestAccounts' }), _ => '');
        }
        else {
            swal("MetaMask not found!", "Install the MetaMask browser extension.", "error", {
                buttons: {
                    cancel: "Cancel",
                    install: {
                        text: "Install MetaMask",
                        value: "install"
                    },
                }
            }).then((value) => {
                if (value == "install") {
                    window.open("https://metamask.io/", '_blank').focus();
                }
            });
        }
    },

    MetaMaskReloadPageOnAccountChange: function () {
        var handler = function (whatChanged) {
            swal("Invalid " + whatChanged + " detected", 'Please reload the game', "info", { button: "Reload" })
                .then(_ => window.location.reload());
        }

        ethereum.on('accountsChanged', _ => handler('account'));
        ethereum.on('chainChanged', _ => handler('chain'));
    },

    MetaMaskWalletId: function () {
        return requestHandler.handleRequest(ethereum.request({ method: 'eth_accounts' }), result => result[0]);
    },

    $connectionState: {
        contract: null,
        // TODO: this shouldn't be hardcoded this way,
        // can we somehow pull this from the smart contract repo?
		abi: [
            'function saveNickname(string memory _nickname) external',
            'function hasNickname() external view returns (bool)',
            'function getNickname() external view returns (string memory)',
            `function enterWithBet(address _token, uint256 _amount) external returns (uint256)`,
            `function getPayoutForBet(address _token, uint256 _amount) external view returns (uint256)`,
            `function playerCurrentlyBetting(address _player) external view returns (bool)`,
            `function tokenIsAllowed(address _token) external view returns (bool)`,
            `function betIsAllowed(address _token, uint256 _amount) external view returns (bool)`,
            `function getAllowedTokens() external view returns (address[] memory)`,
            `function getAllowedBets(address _token) external view returns (uint256[] memory)`
        ],
        address: '0x67ad7058ceB83b0522fE59dE03294bAf4fA5BecA',
        tokenAddress: `0xa362893f40fcC2430b4D95c6f420E923d7d793E2`,
        chainId: 80001, // 80001 is Mumbai, see more at https://chainlist.org/
        userFriendlyChainName: 'Mumbai',
    },

    ABIInit: function () {
        const provider = new ethers.providers.Web3Provider(window.ethereum);
        const signer = provider.getSigner();
        connectionState.contract = new ethers.Contract(connectionState.address, connectionState.abi, signer);
    },

    ABICheckNetworkConnection: function () {
        // https://docs.ethers.io/v5/api/providers/provider/#Provider-getNetwork
        var validateNetwork = function (network) {
            if (network.chainId != connectionState.chainId)
                return 'Invalid chain connected. Please change the chain to ' + connectionState.userFriendlyChainName + ' in your provider settings.';
            return '';
        }
        var promise = connectionState.contract.provider.getNetwork();
        return requestHandler.handleRequest(promise, validateNetwork);
    },

    ABISetNickname: function (nick) {
        var nickAsString = UTF8ToString(nick);
        var promise = connectionState.contract.saveNickname(nickAsString)
            .then(transactionResponse => {
                transactionResponse.wait();
            });

        return requestHandler.handleRequest(promise, _ => nickAsString);
    },

    ABIGetNickname: function () {
        var promise = connectionState.contract.getNickname();
        return requestHandler.handleRequest(promise, response => response);
    },

    ABIGetAllowedTokens: function () {
        var addressArray = connectionState.contract.getAllowedTokens();
        return requestHandler.handleRequest(addressArray, response => response[0]);
    },

    ABIEnterWithBet: function (amount) {		
        var promise = connectionState.contract.enterWithBet(connectionState.tokenAddress, ethers.utils.parseUnits(UTF8ToString(amount), 18))
            .then(result => result.wait())
			.then(waitResult => {
                var eventIndex = 2; // this comes from smart contract, this is the event with bet data
                var betIdSubstringLength = 2 + 64; // first 66 chars are the bet id, including the 0x prefix
                var transactionIndex = waitResult.events[eventIndex].data.substr(0, betIdSubstringLength); 
                var transactionHash = waitResult.events[eventIndex].transactionHash;
                var contractAddress = waitResult.events[eventIndex].address;

                var json = JSON.stringify({ id: transactionIndex, chain_id: connectionState.chainId, tx: transactionHash, contract: contractAddress });

                return json;
            });
		
		
        return requestHandler.handleRequest(promise, response => response);
    },

    ABIGetPayout: function (betAmount) {
        var etherValue = ethers.utils.parseUnits(UTF8ToString(betAmount), 18);
        var promise = connectionState.contract.getPayoutForBet(connectionState.tokenAddress, etherValue);
        return requestHandler.handleRequest(promise, ethers.utils.formatEther);
	},
	
	ABIBetValidate: function (betAmount) {
        var etherValue = ethers.utils.parseUnits(UTF8ToString(betAmount), 18);
        var promise = connectionState.contract.betIsAllowed(connectionState.tokenAddress, etherValue);
        return requestHandler.handleRequest(promise, ethers.utils.formatEther);
    },

    $tokenState: {
        contract: null,
        abi: [
            `function approve(address spender, uint256 value) external returns (bool)`,
            `function allowance(address owner, address spender) external view returns (uint256)`,
            `function balanceOf(address who) external view returns (uint256)`,
        ],
        tokenAddress: `0xa362893f40fcC2430b4D95c6f420E923d7d793E2`,
        approvalTokenAmount: `100`,
        betContractAddress: `0x67ad7058ceB83b0522fE59dE03294bAf4fA5BecA`,
    },

    TokenABIInit: function () {
        const provider = new ethers.providers.Web3Provider(window.ethereum);
        const signer = provider.getSigner();
        var tokenAddress = ethers.utils.getAddress(tokenState.tokenAddress);
        tokenState.contract = new ethers.Contract(tokenAddress, tokenState.abi, signer);
    },

    TokenABIApprove: function () {
        var betContractAddress = ethers.utils.getAddress(tokenState.betContractAddress);

        var promise = tokenState.contract.approve(betContractAddress, ethers.utils.parseEther(tokenState.approvalTokenAmount, 18))
            .then(transactionResponse => transactionResponse.wait());

        return requestHandler.handleRequest(promise, response => response);
    },

    TokenABIIsAllowed: function (walletAddress) {
        var accountAddress = ethers.utils.getAddress(UTF8ToString(walletAddress));
        var betContractAddress = ethers.utils.getAddress(tokenState.betContractAddress);

        var promise = tokenState.contract.allowance(accountAddress, betContractAddress)
            .then(result => {
                var returnedValue = ethers.utils.formatEther(result);
                console.log('Token Allowed result:');
                console.log(returnedValue);
                return returnedValue;
            });

        return requestHandler.handleRequest(promise, response => response);
    },

    TokenABIBalance: function (walletAddress) {
        var accountAddress = ethers.utils.getAddress(UTF8ToString(walletAddress));

        var promise = tokenState.contract.balanceOf(accountAddress)
            .then(result => {
                var returnedValue = ethers.utils.formatEther(result);
                console.log('Token Allowed result:');
                console.log(returnedValue);
                return returnedValue;
            });

        return requestHandler.handleRequest(promise, response => response);
    },
}

autoAddDeps(blockchainIntegration, '$requestHandler');
autoAddDeps(blockchainIntegration, '$connectionState');
autoAddDeps(blockchainIntegration, '$tokenState');
mergeInto(LibraryManager.library, blockchainIntegration);
