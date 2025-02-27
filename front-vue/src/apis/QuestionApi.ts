import axios from "axios";
import { QuestionList, QuestionListDTO } from "../models/question/QuestionList";
import { useAuthenticationStore } from "../stores/authentication";
import {
  QuestionStats,
  QuestionStatsDTO,
} from "../models/question/QuestionStats";

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

  static async getQuestionStats(id: string): Promise<QuestionStats> {
    const response = await axios.get<QuestionStatsDTO>(
      `${this.API_URL}/${id}/stats`,
      {
        headers: { Authorization: "Bearer " + this.authStore.token },
        responseType: "json",
      }
    );

    return new QuestionStats(response.data);
  }
}
