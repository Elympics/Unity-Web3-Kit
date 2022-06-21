mergeInto(LibraryManager.library, {

	ChangeURL: function(newURL) {
		window.history.pushState("", "", UTF8ToString(newURL));
	},

});