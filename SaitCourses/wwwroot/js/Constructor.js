    var canvas = new fabric.Canvas('canvas');
    document.getElementById('file').addEventListener("change", function (e) {
        var file = e.target.files[0];
    var reader = new FileReader();
        reader.onload = function (f) {
            var data = f.target.result;
            fabric.Image.fromURL(data, function (img) {
                var oImg = img.set({left: 0, top: 0, angle: 0, opacity: 0.85 }).scale(0.9);
    canvas.add(oImg).renderAll();
    var a = canvas.setActiveObject(oImg);
                var dataURL = canvas.toDataURL({format: 'png', quality: 0.8 });
});
};
reader.readAsDataURL(file);
});


canvas.setBackgroundImage('https://res.cloudinary.com/itr/image/upload/v1555868664/white_zkntjk.png', canvas.renderAll.bind(canvas));
canvas.setOverlayImage('https://res.cloudinary.com/itr/image/upload/v1555868663/fon_qkezpa.png', canvas.renderAll.bind(canvas));

    function check() {
        var rarr = document.getElementsByName("r");
        if (rarr[0].checked) {
        canvas.setBackgroundImage('https://res.cloudinary.com/itr/image/upload/v1555868664/white_zkntjk.png', canvas.renderAll.bind(canvas));
    canvas.setOverlayImage('https://res.cloudinary.com/itr/image/upload/v1555868663/fon_qkezpa.png', canvas.renderAll.bind(canvas));
}
        else {
        canvas.setBackgroundImage('https://res.cloudinary.com/del5wrr12/image/upload/v1572230069/shirtWoman_yjpuzc.png', canvas.renderAll.bind(canvas));
    canvas.setOverlayImage('https://res.cloudinary.com/del5wrr12/image/upload/v1572230069/shirtWomanBackground_mt4flk.png', canvas.renderAll.bind(canvas));
}

}



var inp = document.querySelector("#file")
inp.value = "";

// var fonts = ["Pacifico", "VT323", "Quicksand", "Inconsolata"];

    function text() {
        var textbox = new fabric.Textbox('This is a Textbox', {
        left: 250,
    top: 250,
    fill: '#000',
    strokeWidth: 2,
    opacity: 0.85,
    stroke: "#000"
});
canvas.add(textbox);
}
    function f() {
        var activeObject = canvas.getActiveObject();
    canvas.remove(activeObject);
}

    function addShirtText(text) {
        if (!text) {
            return;
}
        const shirtText = new fabric.Textbox(text, {
        left: 100,
    top: 100,
    fill: '#000',
    strokeWidth: 2,
    opacity: 0.85,
    stroke: '#000',
    fontFamily: 'Arial',
});
console.log(shirtText);
canvas.add(shirtText);
}

    //  removeShirtText() {
        //  const activeObject = this.preview.getActiveObject();
        //  if (!activeObject || !activeObject._text) {
        //    return;
        //  } else {
        //    this.preview.remove(activeObject);
        //  }
        //}

        function changeShirtTextFont(font) {
            const activeObject = this.preview.getActiveObject();
            if (!activeObject) {
                return;
            } else {
                activeObject.set(
                    {
                        fontFamily: font,
                    },
                );
                this.preview.renderAll();
            }
        }

    //changeShirtTextColor(color){
    //  const activeObject = this.preview.getActiveObject();
    //  if (!activeObject) {
    //    return;
    //  } else {
    //    activeObject.set(
    //      {
    //        fill: color,
    //        stroke: color,
    //      },
    //    );
    //    this.preview.renderAll();
    //  }
    //}



    function changeImageFilter(filter) {
            const activeObject = canvas.getActiveObject();
            var rarr = document.getElementsByName("filter");
            if (!activeObject || activeObject._cacheCanvas) {
                return;
            } else {
                switch (filter) {
                    case 'blackAndWhite': {
                        activeObject.filters.push(new fabric.Image.filters.Saturation({ saturation: -1 }));
                        break;
                    }
                    case 'sepia': {
                        activeObject.filters.push(new fabric.Image.filters.Sepia());
                        break;
                    }
                    case 'pixelate': {
                        activeObject.filters.push(new fabric.Image.filters.Pixelate({ blocksize: 16 }));
                        break;
                    }
                    case 'none': {
                        activeObject.filters = [];
                        break;
                    }
                }
                activeObject.applyFilters();
                canvas.requestRenderAll();
            }
        }

    //fonts.unshift('Times New Roman');

    //var select = document.getElementById("font-family");
    //fonts.forEach(function (font) {
    //    var option = document.createElement('option');
    //    option.innerHTML = font;
    //    option.value = font;
    //    select.appendChild(option);
    //});

    //// Apply selected font on change
    //document.getElementById('font-family').onchange = function () {
    //    if (this.value !== 'Times New Roman') {
    //        loadAndUse(this.value);
    //    } else {
    //        canvas.getActiveObject().set("fontFamily", this.value);
    //        canvas.requestRenderAll();
    //    }
    //};

    //function loadAndUse(font) {
    //    var myfont = new FontFaceObserver(font)
    //    myfont.load()
    //        .then(function () {
    //            // when font is loaded, use it.
    //            canvas.getActiveObject().set("fontFamily", font);
    //            canvas.requestRenderAll();
    //        }).catch(function (e) {
    //            console.log(e)
    //            alert('font loading failed ' + font);
    //        });
    //}

    // Load all

    var red = new fabric.Rect({
        top: 0,
    left: 0,
    width: 500,
    height: 500,
    opacity: 0.70,
    fill: 'white'
});
canvas.add(red);
canvas.item(0).selectable = false;


var tscolor = document.getElementById("tcolor");
var theInput = document.getElementById("favcolor");
    tscolor.addEventListener("input", function () {
        var theColor = tscolor.value;
    var activeObject = canvas.getActiveObject();
    var d = canvas.item(0);
        d.set({
        fill: theColor,
    stroke: theColor
});
canvas.renderAll();
}, true);
    theInput.addEventListener("input", function () {
        var theColor = theInput.value;
    var activeObject = canvas.getActiveObject();
        activeObject.set({
        fill: theColor,
    stroke: theColor
});
canvas.renderAll();
}, true);

const cloudName = 'del5wrr12';
const unsignedUploadPreset = 'wschzlnr';

    function uploadFile(file) {
        var url = `https://api.cloudinary.com/v1_1/${cloudName}/upload`;
    var xhr = new XMLHttpRequest();
    var fd = new FormData();
    xhr.open('POST', url, true);
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    // Reset the upload progress bar
    document.getElementById('progress').style.width = 0;
    // Update progress (can be used to show progress indicator)
        xhr.upload.addEventListener("progress", function (e) {
            var progress = Math.round((e.loaded * 100.0) / e.total);
    document.getElementById('progress').style.width = progress + "%";
            console.log(`fileuploadprogress data.loaded: ${e.loaded},
  data.total: ${e.total}`);
          });
        xhr.onreadystatechange = function (e) {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var response = JSON.parse(xhr.responseText);
    //// https://res.cloudinary.com/cloudName/image/upload/v1483481128/public_id.jpg
    var url = response.secure_url;
    document.getElementById("imageLink").setAttribute('value', url);
    $('#imageLink2').click();
    var tokens = url.split('/');
    tokens.splice(-2, 0, 'w_150,c_scale');
    var alt = response.public_id;
}
};
fd.append('upload_preset', unsignedUploadPreset);
fd.append('tags', 'browser_upload'); // Optional - add tag for image admin in Cloudinary
fd.append('file', file);
xhr.send(fd);
}
    function save() {
        const file = new File([canvas.toSVG()], 'filename', {type: 'image/sv+xml' });
    uploadFile(file)
}