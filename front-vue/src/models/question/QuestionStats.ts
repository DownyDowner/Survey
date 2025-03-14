import { ChoiceStats, ChoiceStatsDTO } from "./ChoiceStats";

export class QuestionStatsDTO {
  id = "";
  name = "";
  beginDate = "";
  endDate = "";
  multiple = false;
  choices: ChoiceStatsDTO[] = [];
}

export class QuestionStats extends QuestionStatsDTO {
  constructor(data: QuestionStats | QuestionStatsDTO | null) {
    super();
    if (data) {
      Object.assign(this, data);
      this.choices = data.choices.map((c) => new ChoiceStats(c));
    }
  }
}
