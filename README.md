## Trunfo para celular

Esse é um projeto de um jogo de trunfo para celular feito durante o curso de Ciências da Computação na disciplina de Laboratório de Dispositivos Móveis.

# Git

É possível que seja necessário do Git LFS para poder dar pull. A especificação fica em https://git-lfs.github.com/

# Build

A versão do Unity é 2020.3.13f1.
Tenha os módulos para build de Android e iOS instalada, isso pode ser feito pelo Unity Hub.
Deixar o Target API Level como Automatic em Player Preferences.

# Firebase

Cada membro da equipe que for trabalhar com Firebase tem que registrar sua chave SHA-1 no Firebase Console, para fazer isso:

   ```1- Encontre sua chave SHA-1:
     1.1 - Em Windows:
       1.1.1 - Abra o cmd.
       1.1.2 - Navegue até a pasta em que o módulo para build em Android foi instalada.
       1.1.3 - rode o comando keytool -list -v -alias androiddebugkey -keystore %USERPROFILE%\.android\debug.keystore.
       1.1.4 - caso tenha um erro, confira se existe o arquivo debug.keystore no caminho especificado.
     1.2 - Em Linux ou MAC:
       1.2.1 - Abra o cli.
       1.2.2 - Navegue até a pasta em que o módulo para build em Android foi instalada.
       1.2.3 - rode o comando keytool -list -v -alias androiddebugkey -keystore ~/.android/debug.keystore.
   2 - Abra o Firebase Console e abra o Projeto
   3 - Abra o Project Settings e vá até "Seus Aplicativos" e selecione Super Trunfo.
   4 - Na seção de Configuração do SDK, adiciona um impressão digital com a chave que encontrou no passo 1.```


