# BastardSword
A simple bastard sword modeled using teachingCG's project as base.

##Authors: 
# Gabriel Fernando Martín Fernández C411 
# Miguel Alejandro Asin Barthelemy C411


El objeto que decidimos recrear fue una espada bastarda. Nos basamos en una imagen donde se muestran dos modelos distintos de este tipo de espada y la vaina de una de ellas. Por simplicidad y debido a la dificultad de generar todas las geometrías de los múltiples objetos (ya de por si complejos) involucrados en esta imagen se recreó solamente la primera de las espadas. Se trató de ser lo más fiel posible a la forma y composición de la espada mientras que se modificaron algunos aspectos principalmente respecto a las texturas como el acabado y brillo del metal y el piso de madera. 


## Geometría y generación de Meshes

La espada fue modelada usando meshes triangulares generados mediante varios métodos distintos como curvas de bezier, generatrices, curvas en revolución, etcétera. La espada fue dividida en varias partes para hacer más simple su modelado: pomel, mango, protector, guardia de cruz y la hoja. Además antes de proceder a modelar cada componente se crearon varios métodos que generaban figuras relativamente sencillas como triangulos, elipsoides, rectángulos entre otros, que servirían como base para generar las futuras componentes de mayor complejidad. Cada parte se modeló usando una técnica en específico o una combinación de estas según la geometría que se necesitaba generar, por ejemplo para el pomel se empleó una curva de bezier en revolución para simular la forma de un elipsoide. Todos los métodos empleados para generar estos meshes se encuentran en el archivo Sword.cs en la carpeta Rendering.


## Materiales, textura e iluminación

La espada presentaba dos materiales bastante distintos: partes métalicas un tanto brillantes y piezas de cuero negro. Para ello, además de definir las características aproximadas de estos materiales se importaron texturas en dos dimensiones que describen con cierta similitud a estos materiales. La textura de las partes metálicas es un tanto diferente a la original, principalmente por la gran gamma de texturas metálicas existentes que dependen del tipo de metal y la forma en que este se procesa, en este caso la textura mostrada representa a un tipo acero no tan brillante y más poroso que el de la espada original. El brillo y la capacidad de reflejo del material presentado son aspectos que quedaron deficientes en el modelo final con respecto al original.
Otro factor importante en la imagen generada fue la illuminación pues en la fotografía original se tiene una perspectiva algo lejana de la espada y con una illuminación relativamente uniforme y completa de la escena. Para esto se trasladó el foco de luz de la escena a una posición algo alejada de la espada y con una intensidad acorde a esta distancia para mejorar la illuminación en general y lograr que esta alcanzase una mayor área de la escena.


## Otros detalles de implementación

Para facilitar trabajo con las funciones ya implementadas se realizaron algunas modificaciones y adiciones a estas, principalmente en el trabajo con los meshes. Se debe destacar que se definió el operador + entre los meshes para facilitar la operación de "fusionarlos" que es prácticamente la base sobre la cual construimos estructuras complejas partiendo de otras más simples. Otro cambio a notar fue la orientación de los vectores normales de los meshes triangulares. Debido a las distintas técnicas y formas en la que estos meshes iban a ser generados fue de interés añadir un parámetro para especificar la dirección de estos vectore normales y asi evitar que el exterior de nuestra figura termine siendo la "cara oculta" de los meshes y trunque los resultados esperados.     
