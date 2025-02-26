import axios from "axios";
import { QuestionList, QuestionListDTO } from "../models/question/QuestionList";
import { useAuthenticationStore } from "../stores/authentication";

export abstract class QuestionApi {
  static API_URL = "https://localhost:7223/api/questions";
  static authStore = useAuthenticationStore();

  static async getAllActiveQuestions(): Promise<QuestionList[]> {
    const response = await axios.get<QuestionListDTO[]>(
      `${this.API_URL}/active`,
      {
        headers: { Authorization: "Bearer " + this.authStore.token },
        responseType: "json",
      }
    );

    return response.data.map((d) => new QuestionList(d));
  }

  static async getAllClosedQuestions(): Promise<QuestionList[]> {
    const response = await axios.get<QuestionListDTO[]>(
      `${this.API_URL}/closed`,
      {
        headers: { Authorization: "Bearer " + this.authStore.token },
        responseType: "json",
      }
    );

    return response.data.map((d) => new QuestionList(d));
  }
}
