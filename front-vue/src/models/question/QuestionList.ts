export class QuestionListDTO {
  id = "";
  name = "";
  beginDate = "";
  endDate = "";
}

export class QuestionList extends QuestionListDTO {
  constructor(data: QuestionList | QuestionListDTO | null) {
    super();
    if (data) Object.assign(this, data);
  }
}
