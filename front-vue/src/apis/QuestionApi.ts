import axios from "axios";
import { QuestionList, QuestionListDTO } from "../models/question/QuestionList";
import { useAuthenticationStore } from "../stores/authentication";
import {
  QuestionStats,
  QuestionStatsDTO,
} from "../models/question/QuestionStats";
import { QuestionSave } from "../models/question/QuestionSave";
import { QuestionFull, QuestionFullDTO } from "../models/question/QuestionFull";

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

  static async getQuestionWithUserVotes(id: string): Promise<QuestionFull> {
    const response = await axios.get<QuestionFullDTO>(`${this.API_URL}/${id}`, {
      headers: { Authorization: "Bearer " + this.authStore.token },
      responseType: "json",
    });

    return new QuestionFull(response.data);
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

  static async create(question: QuestionSave): Promise<string> {
    const response = await axios.post(QuestionApi.API_URL, question, {
      headers: { Authorization: "Bearer " + this.authStore.token },
      responseType: "json",
    });

    return response.data;
  }

  static async deleteQuestion(id: string): Promise<void> {
    await axios.delete(`${QuestionApi.API_URL}/${id}`, {
      headers: { Authorization: "Bearer " + this.authStore.token },
      responseType: "json",
    });
  }
}
