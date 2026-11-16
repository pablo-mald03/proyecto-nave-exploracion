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
                "INTRO_BIENVENIDA",
                new EducationalTopic(
                    "BITÁCORA DE MISIÓN",
                    "Bienvenido a la Base Lunar Artemisa. Acércate a las terminales iluminadas para analizar los sistemas de supervivencia.",
                    "Usa el joystick izquierdo para desplazarte y el botón amarillo de acción cuando estés frente a una consola."
                )
            },
            {
                "TOPIC_CONSOLE_OXYGEN",
                new EducationalTopic(
                    "GENERADOR DE OXÍGENO (ECLSS)",
                    "En la Luna no existe atmósfera respirable. Esta unidad separa el oxígeno del agua mediante electrólisis.",
                    "El hidrógeno residual se almacena como combustible auxiliar para los propulsores de maniobra."
                )
            },
            {
                "CONSOLE_GRAVEDAD",
                new EducationalTopic(
                    "GRAVEDAD LUNAR (1.62 m/s²)",
                    "La gravedad en la Luna equivale aproximadamente a una sexta parte (16.6%) de la gravedad terrestre.",
                    "Por eso cada salto alcanza mayor altura y requiere menos impulso vertical que en la Tierra."
                )
            },
            {
                "CONSOLE_RADIACION",
                new EducationalTopic(
                    "ESCUDO CONTRA RADIACIÓN SOLAR",
                    "Al carecer de campo magnético global y atmósfera densa, la superficie lunar recibe radiación cósmica directa.",
                    "Los módulos habitables utilizan recubrimientos de regolito lunar compactado y polietileno de alta densidad."
                )
            },
            {
                "CONSOLE_ENERGIA",
                new EducationalTopic(
                    "PANELES SOLARES Y BATERÍAS",
                    "Un día lunar dura cerca de 29.5 días terrestres: 14 días de luz solar continua y 14 días de oscuridad total.",
                    "Durante la noche lunar, la estación depende por completo de celdas de combustible y baterías térmicas."
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