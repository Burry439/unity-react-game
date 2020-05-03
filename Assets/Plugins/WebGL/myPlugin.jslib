var MyPlugin = {
  _GameOver: function(score) {
    window
    // Within the function we're going to trigger
    // the event within the ReactUnityWebGL object
    // which is exposed by the library to the window.

    ReactUnityWebGL.GameOver(score);
  }

}


mergeInto(LibraryManager.library, MyPlugin);