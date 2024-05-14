mergeInto(LibraryManager.library, {
  SaveScore: function (username, score, key) {
    var _username = UTF8ToString(username);
    var _score = UTF8ToString(score);
    var _key = UTF8ToString(key);

    if (!window.hasOwnProperty("SaveScore")) console.error("Missing functions");
    else window.SaveScore(_username, _score, _key);
  },
  RoundTrigger: function (username, roundNumber, key) {
    if (roundNumber < 1) return;
    var _username = UTF8ToString(username);
    var _key = UTF8ToString(key);

    if (!window.hasOwnProperty("RoundTrigger"))
      console.log("Missing functions");
    else window.RoundTrigger(_username, roundNumber, _key);
  },
  OpenPage: function (url) {
    var _url = UTF8ToString(url);
    if (_url != "") window.location.replace(_url);
  },
});
