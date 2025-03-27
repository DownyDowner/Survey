export class ChoiceFullDTO {
  id = "";
  name = "";
}
export class ChoiceFull extends ChoiceFullDTO {
  constructor(data: ChoiceFull | ChoiceFullDTO | null) {
    super();
    if (data) Object.assign(this, data);
  }
}
