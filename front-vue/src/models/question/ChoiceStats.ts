export class ChoiceStatsDTO {
  id = "";
  name = "";
  voteCount = 0;
}

export class ChoiceStats extends ChoiceStatsDTO {
  constructor(data: ChoiceStats | ChoiceStatsDTO | null) {
    super();
    if (data) Object.assign(this, data);
  }
}
