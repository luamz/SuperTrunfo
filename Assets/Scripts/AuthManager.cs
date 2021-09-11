using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using System.Collections.Generic;

namespace Trunfo
{
    public class AuthManager : MonoBehaviour
    {
        //variaveis do Firebase
        [Header("Firebase")]
        public DependencyStatus dependencyStatus;
        public FirebaseAuth auth;
        public FirebaseUser User;

        //variaveis do Login
        [Header("Login")]
        public TMP_InputField emailLoginField;
        public TMP_InputField passwordLoginField;
        public TMP_Text warningLoginText;
        public TMP_Text confirmLoginText;
        public JogadorStruct jogadorData;

        //variaveis do Register
        [Header("Register")]
        public TMP_InputField usernameRegisterField;
        public TMP_InputField emailRegisterField;
        public TMP_InputField passwordRegisterField;
        public TMP_InputField passwordRegisterVerifyField;
        public TMP_Text warningRegisterText;
        public string jogadorPath;
        [SerializeField] private string characterPath;
        [SerializeField] public bool status = false;

        void Awake()
        {
            //Checa se todas dependencias necessarias para o Firebase estão no sistema
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    //Se estiverem inicializa o Firebase
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
            DontDestroyOnLoad(this.gameObject);
        }

        public void FirestoreConect(string path, object data)
        {
            var firestore = FirebaseFirestore.DefaultInstance;
            firestore.Document(path).SetAsync(data);
        }

        private void InitializeFirebase()
        {
            //Define a instancia do objeto de autenticação
            auth = FirebaseAuth.DefaultInstance;
        }

        //Funçao para o botão de login
        public void LoginButton()
        {
            //Chama a corotina de login passando o email e a senha
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        }

        //Funcao para o botao de cadastro
        public void RegisterButton()
        {
            //Chama a corotina de cadastro passando o email, a senha e o nome de usuario
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        }

        public void UpdateFirestore(string UserId)
        {
            Dictionary<string, object> status = new Dictionary<string, object>
                {
                    { "Status", this.status }
                };
            var firestore = FirebaseFirestore.DefaultInstance;
            firestore.Collection("jogador").Document(UserId).UpdateAsync(status);
        }

        private IEnumerator Login(string _email, string _password)
        {
            //Chama a funçao de sign in do Firebase passando o email e a senha
            var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
            //Espera até que a task complete
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

            if (LoginTask.Exception != null)
            {
                //Se tiver erros lide com eles
                Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                confirmLoginText.text = "";

                string message = "Login Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;
                    case AuthError.UserNotFound:
                        message = "Account does not exist";
                        break;
                }
                warningLoginText.text = message;
            }
            else
            {
                //Usuario agora esta logado
                //Agora pega o resultado
                this.User = LoginTask.Result;
                this.status = true;
                this.jogadorData = new JogadorStruct
                {
                    Name = this.User.DisplayName,
                    UserToken = this.User.UserId,
                    Status = this.status
                };
                UpdateFirestore(this.jogadorData.UserToken);
                Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
                warningLoginText.text = "";
                confirmLoginText.text = "Logged In";
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        private IEnumerator Register(string _email, string _password, string _username)
        {
            if (_username == "")
            {
                //Se o campo do nome de usuario estiver vazio mostra um aviso
                warningRegisterText.text = "Missing Username";
            }
            else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
            {
                //Se a senha não for igual mostra um aviso
                warningRegisterText.text = "Password Does Not Match!";
            }
            else
            {
                //Chama a funçao de sign in do Firebase passando o email e a senha
                var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
                
                //Espera até que a task complete
                yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                if (RegisterTask.Exception != null)
                {
                    //Se tiver erros lide com eles
                    Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                    FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    string message = "Register Failed!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            message = "Email Already In Use";
                            break;
                    }
                    warningRegisterText.text = message;
                }
                else
                {
                    //Usuario agora foi criado
                    //Agora pega o resultado
                    User = RegisterTask.Result;

                    if (User != null)
                    {
                        //Cria o perfil de usuario e define o nome de usuario
                        UserProfile profile = new UserProfile { DisplayName = _username };

                        //Call the Firebase auth update user profile function passing the profile with the username
                        //Chama a função da autenticação do firebase para atualizar o perfilpassando o perfil com o nome de usuario
                        var ProfileTask = User.UpdateUserProfileAsync(profile);
                        //Espera até que a task complete
                        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                        if (ProfileTask.Exception != null)
                        {
                            //Se tiver erros lide com eles
                            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                            FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                            warningRegisterText.text = "Username Set Failed!";
                        }
                        else
                        {
                            //Nome de usuario agora definido
                            //Agora retorne para a tela de login
                            this.jogadorPath = "jogador/" + User.UserId;
                            JogadorStruct jogadorData = new JogadorStruct
                            {
                                Name = User.DisplayName,
                                UserToken = User.UserId,
                                Status = this.status
                            };
                            FirestoreConect(this.jogadorPath, jogadorData);

                            UIManager.instance.LoginScreen();
                            warningRegisterText.text = "";
                        }
                    }
                }
            }
        }
    }
}