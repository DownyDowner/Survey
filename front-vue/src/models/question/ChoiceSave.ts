export class ChoiceSaveDTO {
  name = "";
}

export class ChoiceSave extends ChoiceSaveDTO {
  constructor(data: ChoiceSave | ChoiceSaveDTO | null) {
    super();
    if (data) {
      Object.assign(this, data);
    }
  }
}
