# SemanticVersionComparer

Descripción

Proyecto pequeño que contiene la clase `Comparer` en el espacio de nombres `SemanticVersionComparer`. Proporciona la función estática `Compare` que recibe dos cadenas con versiones semánticas (por ejemplo `1.2.3`, `1.0.0-alpha.1`) y devuelve la versión considerada mayor.

Ejemplo de uso
```
using SemanticVersionComparer;
public void CompareVersions(){
  string v1 = "1.0.0"
  string v2 = "4.5.1"

  string latestVer = Comparer.Compare(v1, v2);
}
```

Comportamiento y notas

- Se comparan `major`, `minor` y `patch` numéricamente.
- Para pre-releases, la implementación reconoce `alpha`, `beta` y `rc` aplicando el orden: `alpha < beta < rc`.
- Si una versión carece de etiqueta pre-release y la otra la tiene, la versión sin etiqueta se considera mayor.
- Si la etiqueta de pre-release no encaja con el patrón reconocido, se compara por cadena.
