$(function () {
  var topic = $.connection.topicHub;

  // Callbacks to the client should be defined here.
  topic.client.notify = function () {

  };

  $.connection.hub.start().done(function () {
    // Calls to server should be included within the hub.start() method.
/*
    $('#btnSubmit').click(function () {
      // Call the ContactFormSubmitted method on the hub.
      topic.server.contactFormSubmitted($('#txtName').val(), $('#txtComments').val());
      // Clear text boxes and reset focus for next comment.
      $('#txtName').val('');
      $('#txtComments').val('');
      return false;  // prevent 'form' from submitting.
    });
*/
  });
});