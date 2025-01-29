mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
  },

  YandexSDKIsEnabled: function () {
    return false;
  },

  ShowFullscreenAdv: function () {
    ysdk.adv.showFullscreenAdv({
        callbacks: {
            onClose: function(wasShown) {

            },
            onError: function(error) {
              // some action on error
            }
        }
    })
  },

  UpdateLeaderboardScore: function (value) {
    ysdk.getLeaderboards()
          .then(lb => {
            lb.setLeaderboardScore('HighScore', value);
            console.log('setLeaderboardScore', value);
          });
  },

  // GetLang: function () {
  //   var lang = ysdk.environment.i18n.lang;
  //   var bufferSize = lengthBytesUTF8(lang) + 1;
  //   var buffer = _malloc(bufferSize);
  //   stringToUTF8(lang, buffer, bufferSize);
  //   return buffer;
  // },

});