using System.Collections.Generic;

namespace NaveExploracion.Data
{

    //Struct model
    public struct EducationalTopic
    {
        public string Title;
        public string[] Pages;

        public EducationalTopic(string title, params string[] pages)
        {
            Title = title;
            Pages = pages;
        }
    }

    public static class StationDialogueDatabase
    {
        public const string TOPIC_INTRO_BIENVENIDA = "INTRO_BIENVENIDA";
        public const string TOPIC_CONSOLE_OXYGEN = "TOPIC_CONSOLE_OXYGEN";
        public const string TOPIC_CONSOLE_GRAVEDAD = "CONSOLE_GRAVEDAD";
        public const string TOPIC_CONSOLE_RADIACION = "CONSOLE_RADIACION";
        public const string TOPIC_CONSOLE_ENERGIA = "CONSOLE_ENERGIA";

    // Dialogs for the game
    public static readonly Dictionary<string, EducationalTopic> Entries = new Dictionary<string, EducationalTopic>
    {
            {
                "CURIOSIDAD_HUELLAS",
                new EducationalTopic(
                    "HUELLAS ETERNAS EN LA LUNA",
                    "En la Luna no existe viento, lluvia ni ríos que erosionen el suelo como en la Tierra.",
                    "Por eso, las pisadas que dejaron los astronautas del Apolo seguirán intactas durante millones de años."
                )
            },
            {
                "CURIOSIDAD_BANDERA",
                new EducationalTopic(
                    "¿POR QUÉ ONDEA LA BANDERA?",
                    "Como en la Luna no hay aire, la bandera tiene un tubo horizontal arriba para mantenerse estirada.",
                    "Tras décadas bajo la radiación solar sin atmósfera, sus colores probablemente ya se volvieron blancos."
                )
            },
            {
                "CURIOSIDAD_REGOLITO",
                new EducationalTopic(
                    "POLVO CON OLOR A PÓLVORA",
                    "El suelo lunar, llamado regolito, está formado por rocas pulverizadas y afiladas como vidrio molido.",
                    "Los astronautas contaron que al quitarse el casco dentro de la nave, el polvo lunar olía a pólvora quemada."
                )
            },
            {
                "CURIOSIDAD_HIELO_POLAR",
                new EducationalTopic(
                    "HIELO OCULTO EN LOS POLOS",
                    "En el fondo de los cráteres polares nunca entra la luz del Sol, conservando millones de toneladas de hielo.",
                    "Este recurso servirá para obtener agua potable, oxígeno respirable y combustible para futuros cohetes."
                )
            },
            {
                "CURIOSIDAD_RECICLAJE_AGUA",
                new EducationalTopic(
                    "RECICLAJE EXTREMO DE AGUA",
                    "En el espacio no se desperdicia ni una gota: se recolecta el sudor, el vapor al respirar y hasta la orina.",
                    "Tras pasar por filtros avanzados, se convierte en agua más limpia que la del grifo de tu casa."
                )
            },
            {
                "CURIOSIDAD_COMIDA_ESPACIAL",
                new EducationalTopic(
                    "TORTILLAS EN LUGAR DE PAN",
                    "Sin gravedad, las migajas de pan flotan por toda la cabina y pueden dañar los equipos electrónicos.",
                    "Por eso los astronautas prefieren usar tortillas, ya que no sueltan migas al comerlas en el espacio."
                )
            },
            {
                "CURIOSIDAD_TEMPERATURA",
                new EducationalTopic(
                    "CLIMA DE FUEGO Y HIELO",
                    "Sin atmósfera que regule el calor, durante el día lunar el suelo alcanza los 120 °C, ¡suficiente para hervir agua!",
                    "En cambio, durante la noche lunar la temperatura cae hasta los -130 °C de frío extremo."
                )
            },
            {
                "CURIOSIDAD_MAREAS",
                new EducationalTopic(
                    "EL SUPERPODER DE LAS MAREAS",
                    "Aunque está a 384,000 km de distancia, la gravedad de la Luna jala el agua de nuestros océanos.",
                    "Esa atracción invisible hace que el nivel del mar suba y baje dos veces al día en la Tierra."
                )
            },
            {
                "CURIOSIDAD_CARA_OCULTA",
                new EducationalTopic(
                    "SIEMPRE LA MISMA CARA",
                    "La Luna tarda exactamente lo mismo en girar sobre sí misma que en dar una vuelta alrededor de la Tierra.",
                    "Debido a esta sincronía perfecta, desde nuestro planeta siempre vemos el mismo lado lunar."
                )
            },
            {
                "CURIOSIDAD_CAIDA_LIBRE",
                new EducationalTopic(
                    "EL MARTILLO Y LA PLUMA",
                    "En la Luna no hay resistencia del aire que frene los objetos ligeros cuando caen hacia el suelo.",
                    "En el Apolo 15 comprobaron que un martillo y una pluma soltados a la vez tocan el suelo juntos."
                )
            }
        };

        //Principal topic return data
        public static EducationalTopic GetTopic(string topicId)
        {
            if (Entries.TryGetValue(topicId, out EducationalTopic topic))
            {
                return topic;
            }

            return new EducationalTopic(
                "DATOS NO DISPONIBLES",
                $"No se encontró información registrada para el identificador: {topicId}."
            );
        }
    }
}