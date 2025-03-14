export class ChoiceFullDTO {
  id = ""; // GUID en tant que chaîne
  name = "";
}

// Classe pour le choix avec un constructeur qui assigne les propriétés
export class ChoiceFull extends ChoiceFullDTO {
  constructor(data: ChoiceFull | ChoiceFullDTO | null) {
    super();
    if (data) {
      Object.assign(this, data); // Assigner les propriétés de `data` à `this`
    }
  }
}
