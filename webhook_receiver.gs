function doPost(e) {
  Logger.log(e)
  return ContentService.createTextOutput(e);
}

function createWebhook(){
  const teamId = '9014207635';
  const url = `https://api.clickup.com/api/v2/team/${teamId}/webhook`
  var options = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'pk_82322956_M0PHADY6E7T7KDTV8364W7L4BOJO3RFM'
    },
    body: JSON.stringify({
      endpoint: 'https://script.google.com/a/macros/eduspecialistonline.org/s/AKfycbwOBX_fcRKiPJeN82Yubve-AETOwr-BvwLmryxpbUB00ywK4fFPkRBxcFrq479V38N5/exec',
      events: [
        'taskAssigneeUpdated',
      ],
      space_id: 90141022589,
      list_id: 901402709756,
    })
  }

  var response = UrlFetchApp.fetch(url, options);
  var responseData = response.getContentText();
  var json = JSON.parse(responseData);
}