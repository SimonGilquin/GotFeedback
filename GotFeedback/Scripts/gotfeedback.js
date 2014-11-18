$(function () {
  var topic = $.connection.topicHub;
  var notificationsMenu = $('#NotificationsMenu');
  var notifications = $('#Notifications');

  function addNotification(title, link, desription) {
    var notif = '<li class=""><a href="' + link + '">' + title + '</a></li>';
    if ($('.empty', notifications).length > 0) {
      notifications.html(notif);
      notificationsMenu.append('<span class="badge">1</span>');
    } else {
      notifications.prepend(notif);
      var count = $('.badge', notificationsMenu);
      count.text(parseInt(count.text()) + 1);
    }
  }

  // Callbacks to the client should be defined here.
  topic.client.notify = function (topic) {
    addNotification('New comment on ' + topic.topic, '/Topics/Details/' + topic.topicId + '#comment-' + topic.commentId, topic.comment);
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

  $('#TopicDetails').on('submit', '#NewCommentForm', function(e) {
    e.preventDefault();
    var form = $(this);
    var commentField = $('input[name="Message"]', form);
    var button = $('input[type="submit"]', form);
    commentField.attr('disabled', 'disabled');
    button.attr('disabled', 'disabled');
    $.post(form.attr('action'), form.serialize()).done(function (data, status, xhr) {
      $('#TopicComments').html($('#TopicComments', data).html());
      commentField.val('');
      commentField.removeAttr('disabled');
      button.removeAttr('disabled');
    });

    return false;
  });
});