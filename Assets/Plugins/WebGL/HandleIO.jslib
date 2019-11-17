var HandleIO = {
    WindowAlert : function(message)
    {
        window.alert(Pointer_stringify(message));
    },
    SyncFiles : function()
    {
        FS.syncfs(false,function (err) {
            // handle callback
        });
    },
    // firebase Initialize: Creates and initializes a Firebase app instance.
    firebaseInit : function(){
   
        const firebaseConfig = {
            apiKey: "AIzaSyDu6u3FQA8lBzMWEm6VAXgBblWHPmAupoI",
            authDomain: "rbowebgl.firebaseapp.com",
            databaseURL: "https://rbowebgl.firebaseio.com",
            projectId: "rbowebgl",
            storageBucket: "rbowebgl.appspot.com",
            messagingSenderId: "182947139092",
            appId: "1:182947139092:web:466b459c45775504bd2101",
            measurementId: "G-QSL5XTH28T"
        };
        if (!firebase.apps.length) {
             firebase.initializeApp(firebaseConfig);
        }
    },
    // Create the authenticated credential
    createAuthenticationUI : function()
    {
        window.authenticated = 0;
        var uiConfig = {
            callbacks: {
                signInSuccessWithAuthResult: function(authResult, redirectUrl) {
                    var user = authResult.user;
                    var credential = authResult.credential;
                    var isNewUser = authResult.additionalUserInfo.isNewUser;
                    var providerId = authResult.additionalUserInfo.providerId;
                    var operationType = authResult.operationType;
                    window.authenticated = 1;
                    window.user = authResult.user;
                    unityInstance.SendMessage('Authentication', 'RemoveLoadingText');
                    unityInstance.SendMessage('Authentication', 'LoadLandingScene');
                    // after authentication populate the fiilename list in LoadManager.fileNames
                    unityInstance.Module.asmLibraryArg._getBucketFiles();
                    
                    // Do something with the returned AuthResult.
                    // Return type determines whether we continue the redirect automatically
                    // or whether we leave that to developer to handle.
                    return false;
                },
                signInFailure: function(error) {
                    // Some unrecoverable error occurred during sign-in.
                    // Return a promise when error handling is completed and FirebaseUI
                    // will reset, clearing any UI. This commonly occurs for error code
                    // 'firebaseui/anonymous-upgrade-merge-conflict' when merge conflict
                    // occurs. Check below for more details on this.
                    return handleUIError(error);
                },
                uiShown: function() {
                    // The widget is rendered.
                    // Hide the loader.
                    document.getElementById('loader').style.display = 'none';
                    unityInstance.SendMessage('Authentication', 'RemoveLoadingText');
                }
            },
            signInSuccessUrl: 'loggedIn.html',
            signInOptions: [
            // Leave the lines as is for the providers you want to offer your users.
            firebase.auth.GoogleAuthProvider.PROVIDER_ID,
            firebase.auth.EmailAuthProvider.PROVIDER_ID
            ],
            // tosUrl and privacyPolicyUrl accept either url string or a callback
            // function.
            // Terms of service url/callback.
            //tosUrl: '<your-tos-url>',
            // Privacy policy url/callback.
            //privacyPolicyUrl: function() {
           // window.location.assign('<your-privacy-policy-url>');
            //}
        };

        // Initialize the FirebaseUI Widget using Firebase.
        var ui = new firebaseui.auth.AuthUI(firebase.auth());
        // The start method will wait until the DOM is loaded.
        ui.start('#firebaseui-auth-container', uiConfig);
    },
    // set the position of the authenticationSignIn botton
    setUIDiv : function()
    {
        var div = document.createElement("firebaseui-auth-container");
        div.id = "firebaseui-auth-container";
        div.style.paddingTop = "45px";
        div.style.paddingLeft = "45px";
        div.style.zIndex = "100";
        div.style.position = "absolute";
        document.getElementsByTagName("body")[0].appendChild(div);
    },
    // authenticationSignIn botto
    authenticationSignIn : function()
    {
        // Using a popup.
        var provider = new firebase.auth.GoogleAuthProvider();
        provider.addScope('profile');
        provider.addScope('email');
        firebase.auth().signInWithPopup(provider).then(function(result) {
            // This gives you a Google Access Token.
            var token = result.credential.accessToken;
            // The signed-in user info.
            var user = result.user;
        });
    },
    // return true if authentication is succeful
    verifyAuthentication : function()
    {
        var user = firebase.auth().currentUser;
        if (user)
            return true;
        else
            unityInstance.SendMessage('Authentication', 'DisplayLoadingText');
        return false;
    },
    // return the authentication user ID
    getUserIDString : function()
    {
        if (window.user !== "undefined")
        {
            var bufferSize = lengthBytesUTF8(window.user.displayName) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(window.user.displayName, buffer, bufferSize);
            return buffer;
        }
        return "undefined";
    },
    // get the list of the buckets file path
    getBucketFiles: function(){

//      Get a reference to the storage service, which is used to create references in your storage bucket
        var storage = firebase.storage();

        //Create a storage reference from our storage service
        var storageRef = storage.ref();
        // Create a reference under which you want to list
        var listRef = storageRef.child('alldatas');

        // Find all the prefixes and items.
        listRef.listAll().then(function(res) {
        res.prefixes.forEach(function(folderRef) {
            // All the prefixes under listRef.
            // You may call listAll() recursively on them.
        });
        res.items.forEach(function(itemRef) {
            // All the items under listRef.
            var path = itemRef.name;
            var bufferSize = lengthBytesUTF8(itemRef.name) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(path, buffer, bufferSize);
            // Call C# function LoadFilePaths in LoadManager to load the file list for the table view
            unityInstance.SendMessage('LoadManager', 'LoadFilePaths', path);
        });
    }).catch(function(error) {
        // Uh-oh, an error occurred!
    });
    },
    // Download the Protobut file
    getProtoData: function(fileName){
        
        var jsFileName = Pointer_stringify(fileName); 
        var path = "alldatas/".concat(jsFileName);
        //      Get a reference to the storage service, which is used to create references in your storage bucket
        console.log("path of the file: " + path);
        var storage = firebase.storage();

        //Create a storage reference from our storage service
        var storageRef = storage.ref()
        storageRef.child(path).getDownloadURL().then(function(url) {
            console.log("JS->Proto url: " + url);
            unityInstance.SendMessage('LoadManager', 'LoadProtoUrl', url); 
           // var bufferSize = lengthBytesUTF8(url) + 1;
           // var buffer = _malloc(bufferSize);
           // stringToUTF8(url, buffer, bufferSize);
           // return buffer;
        }).catch(function(error) {
            // Handle any errors from Storage
            console.log("Request getProtoData Fail");
        });
    },
    // Upload the file to the bucket
    uploadFile : function(data, filename){
        console.log("JS Filename: " + filename);
        var jsData = Pointer_stringify(data);
        var jsFileName = Pointer_stringify(filename);

//      Get a reference to the storage service, which is used to create references in your storage bucket
        var storage = firebase.storage();

        //Create a storage reference from our storage service
        var storageRef = storage.ref();
        storageRef.child('alldatas/' + jsFileName + '.b64').putString(jsData).then(function(snapshot) {
        console.log('Uploaded a base64 string!');
       });
    }
};

mergeInto(LibraryManager.library, HandleIO);