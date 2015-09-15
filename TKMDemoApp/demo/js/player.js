var filesToLoad = 0; // to be set in init
var filesLoaded = 0;
var currentTrack = 0;
var speech = {};
var playList = [];

$(document).ready(function () {
    init();
});

function init()
{
    speech = 
        {
            "name": "Koalisyon",
            "tracks": [
                "assets/p00.mp3",
                "assets/p01.mp3",
                "assets/p02.mp3",
                "assets/p03.mp3",
                "assets/p04.mp3",

                "assets/p05-1.mp3",
                "assets/p05-2.mp3",
                "assets/p06.mp3",
                "assets/p07.mp3",
                "assets/p08.mp3",

                "assets/p09.mp3",
                "assets/p10.mp3",
                "assets/p11-1.mp3",
                "assets/p11-2.mp3",
                "assets/p12.mp3",
                "assets/p13.mp3",

                "assets/p14.mp3",
                "assets/p15.mp3",
                "assets/p16.mp3",
                "assets/p17.mp3",
                "assets/p18.mp3",

                "assets/p19.mp3",
                "assets/p20.mp3",
                "assets/p21.mp3",
                "assets/p22.mp3",
                "assets/p23.mp3",

                "assets/p24.mp3",
                "assets/p25.mp3",
                "assets/p26.mp3",
                "assets/p27.mp3",
                "assets/p28.mp3",

                "assets/p29.mp3",
                "assets/p30.mp3",
                "assets/p31.mp3",
                "assets/p32.mp3",
                "assets/p33.mp3",

                "assets/p34.mp3",
                "assets/p35.mp3",
                "assets/p36.mp3",
                "assets/p37.mp3",
                "assets/p38.mp3",

                "assets/p39.mp3",
                "assets/p40.mp3",
                "assets/p41.mp3",
                "assets/p42.mp3",
                "assets/p43.mp3",

                "assets/p44.mp3",
                "assets/p45.mp3",
                "assets/p46.mp3",
                "assets/p47.mp3",
                "assets/p48.mp3",
                "assets/p49.mp3"
            ],
            "captions": [
                "Her kafadan ses geliyor yine",
                "Olur olmaz yükseliyor tansiyon",
                "Herkes önce dönüp baksın kendine",
                "Hareketler ofsayt,sözler sansasyon",
                "Bu kafayla kurulamaz koalisyon",

                "Nasıl isen öyle görünmek gerek",
                "Doğruluk zırhıyla korunmak gerek",
                "Her türlü hileden arınmak gerek",
                "Dostluklar için şart sterilizasyon",
                "Bu kafayla kurulamaz koalisyon",

                "Kimisi ufacık yetki alınca",
                "Acımıyor fırsatını bulunca",
                "Niyetler bağcıyı dövmek olunca",
                "Tabii ki sağlanmaz senkronizasyon",
                "Bu kafayla kurulamaz koalisyon",

                "Sonuçlardan kimse bir ders almazsa",
                "Bir bilenin kapısını çalmazsa",
                "Bu konuda niyet halis olmazsa",
                "Tükenmez elbette spekülasyon",
                "Bu kafayla kurulamaz koalisyon",

                "Kimi çıkıp yeller gibi esiyor",
                "Kimi sallandırıp",
                "Kimi asıyor",
                "Kameraya geçen ahkam kesiyor",
                "Yeter artık bunca prezantasyon",
                "Bu kafayla kurulamaz koalisyon",

                "Gururu, egoyu yenmemiz lazım",
                "Konunun özüne inmemiz lazım",
                "Fıtrat ayarına dönmemiz lazım",
                "Eğer yapılmazsa kalibrasyon",
                "Bu kafayla kurulamaz koalisyon",

                "Herkes asıl niyetini saklıyor",
                "Şartlarına yenisini ekliyor",
                "Haftalardır millet çözüm bekliyor",
                "Biline ki bitmek üzre opsiyon",
                "Bu kafayla kurulamaz koalisyon",

                "Olunursa birazcık haşır neşir",
                "Flu olan yavaş yavaş netleşir",
                "Bizde herkes racon keser, restleşir",
                "Acil ihtiyaç... rehabilitasyon",
                "Bu kafayla kurulamaz koalisyon",

                "Şarttan geçilmiyor öyle zaman ki",
                "İmanın şartları sanırsın sanki",
                "Müzakere böyle olmaz inan ki",
                "Yeter artık bunca dezenformasyon",
                "Bu kafayla kurulamaz koalisyon",

                "Türkiye’dir hepimizin emeli",
                "Birlik olmak meselenin temeli",
                "Önce vatan, önce millet demeli",
                "Bunlardan gayrısı manipülasyon",
                "'Ben' diyerek",
                "Kurulamaz koalisyon"
            ],
            "sentences": [],
            "audio": []
        };

    // preload audio files
    filesToLoad = speech["tracks"].length;
    for (var i = 0; i < speech["tracks"].length; i++)
    {
        var audio = loadAudio(speech["tracks"][i]);
        speech["audio"].push(audio);
       
    }

    // init gui
    initLists();

    // clear subs
    $("#currentCaptionText").text("...");

    // clear count 
    $('#playListCount').text('0');

    // init auto-complete
    $('#typeahead').typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    },
    {
        name: 'captions',
        source: substringMatcher(speech["captions"])
    });

    $('#typeahead').bind('typeahead:select', processSelection); 
    $('#typeahead').typeahead('val', ''); //clear text

    // check playlist from url
    var hashloc = location.hash.indexOf('#');
    if(hashloc!=-1)
    {
        //split and add to playlist and play
        var list = location.hash.slice(hashloc + 1, location.hash.length - hashloc + 1).split(",");
        for (i = 0; i < list.length; i++)
        {
            addToPlaylist(speech["captions"][list[i]]);
        }
        play();
    }
}

function initLists() {
    //track list
    var tList = $("#trackList");
    tList.empty();
    for (var i = 0; i < speech["captions"].length; i++) {
        tList.append('<li class="list-group-item">' + speech["captions"][i] + '</li>');
    }

    $('#trackCount').text(speech["captions"].length);


    // let the tracklist items be draggable
    $("#trackList li").draggable({
        helper: "clone",
        cursor: "move"
    });
    tList.disableSelection();

    // let the playlist be droppable, accepting the gallery items
    var pList = $("#playList");
    pList.empty();
    pList.droppable({
        accept: "#trackList li",
        //connectWith: playList,
        drop: function (event, ui) {
            if (ui.draggable.text() != "")
                addToPlaylist(ui.draggable.text());
        }
    });
    pList.disableSelection();
    pList.sortable();


    // init double click events on lists
    $(document).on('dblclick', '#trackList li', addToPlaylist);
    $(document).on('dblclick', '#playList li', removeFromPlaylist);

}

// ****************************************************
// playlist functions
// ****************************************************
function clearPlaylist()
{
    $("#playList").empty();
    location.hash = "";
    setCaption("...");
    updatePlaylistCount();
}

function addToPlaylist(e)
{
    var txt = ((typeof e) == "string") ? e : $(this).text();
    $("#playList").append('<li class="list-group-item">' + txt + '</li>');
    updatePlaylistCount();
}

function removeFromPlaylist(e)
{
    $(this).remove();
    updatePlaylistCount();
}

function updatePlaylistCount()
{
    $("#playListCount").text($("#playList li").length);
}

// ****************************************************
// audio functions
// ****************************************************
function loadAudio(uri) {
    var audio = new Audio();
   // console.log('loading file: '+uri);
    audio.addEventListener('canplaythrough', updateProgress, false); // progress monitoring
    // audio.addEventListener('ended', nextTrack);
    audio.addEventListener('timeupdate', nextTrack, false);
    audio.src = uri;
    return audio;
}

function updateProgress() {
    //if (filesLoaded >= filesToLoad)
    //    main();
    filesLoaded++;
    var percent = (filesLoaded / filesToLoad) * 100;
    $('.progress-bar').css('width', percent + '%').attr('aria-valuenow', percent);
    if (percent >= 100) {
        $('.progress').fadeOut();
    }
}

function play() {
    // clear all
    currentTrack = 0;
    playList = [];
    var elems = $('#playList li');
    for (i = 0; i < elems.length; i++) {
        $(elems[i]).removeClass("list-inline-selected");
    }

    // get playlist captions
    var list = $('#playList li').map(function (i, el) {
        return $(el).text();
    }).get();

    // find the caption as audio and add to playlist
    for (var k = 0; k < list.length; k++) {
        var caption = list[k];
        var index = speech["captions"].indexOf(caption);
        playList.push(index);
    }
    // start playing if not empty
    if (playList.length != 0) {
        speech["audio"][playList[0]].play();
        location.hash = "#" + playList;
        setCaption(0);
    }
    else {
        setCaption("<strong>Calacak birsey yok...</strong>");
    }
}

function stop() {
    $(speech["audio"][playList[currentTrack]]).trigger('pause');
}

function nextTrack()
{
    var delta = 300;
    var currentTimeMs = speech["audio"][playList[currentTrack]].currentTime * 1000;
    var totalTimeMs = speech["audio"][playList[currentTrack]].duration * 1000;

    // start next track before current track ends --crossfading
    if (currentTimeMs + delta > totalTimeMs && (currentTrack < playList.length - 1)) {
        var nextTrack = currentTrack + 1;
        if (this == speech["audio"][playList[currentTrack]] && speech["audio"][playList[nextTrack]].paused) {
            $(speech["audio"][playList[nextTrack]]).trigger('play');

            // set caption
            setCaption(nextTrack);
            currentTrack = nextTrack;
        }
    }
    //console.log(currentTimeMs);
    //console.log(audio.duration * 1000);
}

// ****************************************************
// GUI functions
// ****************************************************

function setCaption(track) {
    if ((typeof track) == 'number') {
        $("#currentCaptionText").html('<span class="glyphicon glyphicon-headphones pull-left" aria-hidden="true"></span><strong>' + speech["captions"][playList[track]] + '</strong>');

        // highlight current list item in playlist
        var elems = $('#playList li');
        $(elems[currentTrack]).removeClass("list-inline-selected");
        $(elems[track]).addClass("list-inline-selected");
        //for (i = 0; i < elems.length; i++)
        //{
        //    if ($(elems[i]).text() == speech["captions"][playList[currentTrack]]) {
        //        $(elems[i]).removeClass("list-inline-selected");
        //    }
        //    if ($(elems[i]).text() == speech["captions"][playList[track]]) {
        //        $(elems[i]).addClass("list-inline-selected");
        //    }
        //}

    }
    else {
        $("#currentCaptionText").html(track);
    }
}

function shareWindowPopup() {
    location.hash = "#" + playList;

    swal("Dinlediginiz eserin adresi", location.href);

}

// ****************************************************
// typeahead.js support functions
// ****************************************************
function processSelection(ev, suggestion) {
    // $("#playList").append('<li class="list-group-item">' + suggestion + '</li>');
    addToPlaylist(suggestion);
    $('#typeahead').typeahead('val', '');
    //console.log('Selection: ' + suggestion);
}

var substringMatcher = function (strs) {
    return function findMatches(q, cb) {
        var matches, substringRegex;

        // an array that will be populated with substring matches
        matches = [];

        // regex used to determine if a string contains the substring `q`
        substrRegex = new RegExp(q, 'i');

        // iterate through the pool of strings and for any string that
        // contains the substring `q`, add it to the `matches` array
        $.each(strs, function (i, str) {
            if (substrRegex.test(str)) {
                matches.push(str);
            }
        });

        cb(matches);
    };
};



//function nextTrack() {
//    if (currentTrack > filesToLoad)
//        return;
//    console.log(currentTrack.toString() + ':' + speech["captions"][playList[currentTrack]]);
//    $(speech["audio"][playList[currentTrack++]]).trigger('play'); //animate({ volume: 1.0 }, 100);
//    //  $(loadedFiles[currentTrack++]).animate({ 0.0: 1.0 }, 100);
//}
//# sourceMappingURL=player.js.map
