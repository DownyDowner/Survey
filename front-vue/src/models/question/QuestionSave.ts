import { ChoiceSave, ChoiceSaveDTO } from "./ChoiceSave";

export class QuestionSaveDTO {
  name = "";
  beginDate = "";
  endDate = "";
  multiple = false;
  choices: ChoiceSaveDTO[] = [];
}

export class QuestionSave extends QuestionSaveDTO {
  constructor(data: QuestionSave | QuestionSaveDTO | null) {
    super();
    if (data) {
      Object.assign(this, data);
      this.choices = data.choices.map((c) => new ChoiceSave(c));
    }
  }
}
