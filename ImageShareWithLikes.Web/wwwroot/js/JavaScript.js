$(() => {

    const imageId = $("#image-id").val();

    $("#like-button").on('click', function () {
        $.post('/home/AddLike', { imageId }, () => {
            $("#like-button").attr('disabled', 'true');
        });
    });

    setInterval(() => {
        $.get('/home/getLikes', { imageId }, function (likes) {
            $("#likes-count").text(likes);
        });
    }, 500);





});