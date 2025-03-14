import { ChoiceFull, ChoiceFullDTO } from "./ChoiceFull";

export class QuestionFullDTO {
  id = "";
  name = "";
  beginDate = "";
  endDate = "";
  multiple = false;
  choices: ChoiceFullDTO[] = [];
}

export class QuestionFull extends QuestionFullDTO {
  constructor(data: QuestionFull | QuestionFullDTO | null) {
    super();
    if (data) {
      Object.assign(this, data);
      this.choices = data.choices.map((c) => new ChoiceFull(c));
    }
  }
}
