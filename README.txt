README TRABAJO ENTREPÁGINAS.

Nombre del proyecto-> EntrePáginas

Descripción-> EntrePáginas es una aplicación que te permitirá buscar libros de una amplia
lista de libros que te ofrecemos, permitiendo así que tengas un seguimiento de todos tus libros leídos. EntrePáginas añade dos funcionalidades para más comodidad del usuario las cuales son: Huella Digital, cada usuario se podrá identificar con su huella de manera que se ahorré poner la contraseña cada vez que quiera entrar en su aplicación, siempre que dicho usuario tenga la opción de huella en ese dispositivo. Y secundariamente búsqueda de título por voz, ¿Alguna vez te ha pasado que has visto un libro en una librería que te ha llamado la atención pero quieres saber más información de el?, gracias a nuestra búsqueda por voz te permitirá decir el título de un libro y que automáticamente te lleve hasta el perfil de dicho libro.

He seguido el diseño que hice anteriormente en otro trabajo de figma, en su versión acortada.

Tecnologías usadas-> 
- .NET MAUI, C# y XAML.
- Patrón MVVM (Modelos User y Book y ViewModels y Views InicioSesión, Registro, Buscar, Página Principal, PerfilLibro), uso de Binding Context y de ICommand.
- Reconocimiento de voz (Speech-To-Text) mediante Android Nativo creando un servicio que use recognizerIntent y objetos propios del SDK de Android, integrados dentro del proyecto MAUI.
- Uso de TaskCompletionSource <string> ( Que son como las promesas en JS) para la implementación de reconocimiento de voz.
- Uso de Text-To-Speech.
- Plugin.FingerPrint para la huella y uso de un servicio para seguridad de huella (DevicesWithBiometrics).
- Uso de diccionarios de recursos para estilos.
- Fody y PropertyChanged.
- Plataforma solo en Android.


Problemas encontrados-> Al principio la aplicación estaba pensada para tener un escáner de código de barras para su IBSN en vez de control de voz, cosa que dio muchos problemas ya que al principio la calidad de la cámara era muy mala y cuando se logro tener buena calidad, visual studio no soportaba dicho escaneo expulsándote de la aplicación por lo que lo he dejado en el proyecto (EscanerView como un txt para que no de problemas) para poder tener una futura investigación de dicha tecnología.

Explicación del flujo de proyecto-> 
1-El proyecto inicia en InicioSesiónView que muestra un inicio de sesión corriente, dicho inicio de sesión esta bindeado a su viewmodel, le enviamos el deviceService porque usará el servicio que te hace la función de comprobación de dispositivos añadidos. Una vez que gracias a commands revisa que todo está bien navegará ( viene explicado en el proyecto, necesitamos hacerlo con commands porque hay 2 tipos distintos de navegación, con huella y sin huella ). Si no está bien te tendrás que registrar dándole al botón correspondiente que gracias a su evento clicked te envía a RegistroView. 

2-RegistroView también está bindeado a su ViewModel, como necesita datos del ViewModel de inicio de sesión, se lo pasaremos en el constructor del ViewModel, junto a deviceService. Una vez que te registras gracias al ICommand, hará un pop para que te lleve a la página de Inicio de Sesión. Dicho esto una vez iniciada sesión se hará una nueva navigationpage, esto es para que no se acumulen páginas del inicio de sesión en la pila incorrecta.

3-Ahora nos encontramos en PáginaPrincipalView la cual te muestra los libros que has añadido a tu biblioteca y te permite navegar a buscar. Además tiene un botón que dispara un evento que permite reconocer el título de un libro, esto se hace desde su viewmodel, gracias a que bindeamos su viewmodel a el, además en la instanciación del viewmodel también le pasamos el usuario actual, gracias al inicio de sesión que es el que se encarga de instanciar la vista y pasarle los datos.

4-Si le damos al botón de búsqueda se instanciará una nueva vista Buscar, que tendrá que recoger el usuario también. La vista buscar estará bindeada a su respectivo viewmodel, al cual le pasará el usuario , además este viewmodel contendrá todos los libros que tiene nuestra app y te permitirá buscar por nombre. La vista buscar mostrará todos los libros de la lista en un inicio, permitiéndote ver los detalles de alguno en concreto gracias a un botón "Ver más".

5- Gracias al botón "ver más" podrás ver los detalles de cada libro, esto se hace desde la pantalla de buscar, la cuál define un commandparameter que será el libro donde clickes y un command que servirá para navegar desde él (que instancia una vista de perfil libro, pasándole el usuario y el libro clickado). En la view perfil libro se mostrará los datos del libro en concreto gracias a un binding a su view model (el cual tendrá usuario y libro) y además te permitirá mediante un ICommand añadir un libro a la lista del usuario actual (que ya sabemos bien que recibimos)


Interfaz natural implementada->
- Implemento reconocimiento de voz y Síntesis de voz gracias a Android Nativo más servicio.
Pasos que he hecho:
1- Dar permisos de record_audio en el Androidmanifiest.
2- Crear un servicio de implementación de audio (Primero interfaz fuera de la carpeta Android, luego servicio dentro de la carpeta Android)
3- Capturar resultado en MainActivity que llamará al método del servicio que capturará el resultado.
4- Registrar el servicio en mauiprogram.
5-Usarlo donde quieras.
- Implemento huella digital gracias al Plugin.FingerPrint y control de seguridad gracias a servicio.
Pasos que he hecho:
1- Instalar el plugin.
2- Configurar permisos en el AndroidManifiest de biometric y fingerprint.
3- Crear un servicio de seguridad biométrica (Primero interfaz fuera de la carpeta Android, luego servicio dentro de la carpeta Android)
4- Registrar dicho servicio en mauiprogram.
5- Configurarlo en el mainactivity de Android.
5- Usarlo en donde sea.

Instrucciones para probarlo-> Simplemente necesitas un teléfono Android si puede ser que permita la huella, lo conectas a tu ordenador mediante USB, y activas depuración por usb y instalar aplicaciones por usb, tendrás que tener las opciones de desarrollador activadas para ver dichas posibilidades ( Acerca del teléfono-> Número de compilación->7 veces seguidas sobre el número). Una vez que tienes esto te saldrá en la ejecución de visual estudio, dispositivos locales Android y tendrás que elegir el tuyo. Para lo demás, primero te deberás registrar y iniciar sesión y a continuación podrás ejecutar la función de micrófono, de búsqueda y de añadir libros.Puedes ver el vídeo adjuntado en donde te muestro la funcionalidad básica. 