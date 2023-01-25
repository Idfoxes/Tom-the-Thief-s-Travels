mergeInto(LibraryManager.library,
{
   SaveExtern: function (data)
   {
      var jsonFile = UTF8ToString (data);
      var myData = JSON.parse (jsonFile);
      
      player.setData (myData, true);
      console.log('SAVE GAME DATA: ', myData);
   },
   LoadExtern: function () 
   { 
      player.getData ().then (_data => 
      { 
         console.log('LOADED GAME DATA: ', _data); 
         var myData = JSON.stringify (_data); 
            gameInstance.SendMessage ('world', 'LoadDataFromServer', myData); 
      }); 
   },

   ShowAdv : function (){
      ysdk.adv.showFullscreenAdv({
    callbacks: {
        onClose: function(wasShown) {
          // some action after close
        },
        onError: function(error) {
          // some action on error
        }
        }
         })
   },
});