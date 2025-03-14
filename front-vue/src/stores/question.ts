import { defineStore } from "pinia";
import { Ref, ref } from "vue";
import { QuestionList } from "../models/question/QuestionList";
import { QuestionApi } from "../apis/QuestionApi";
import { QuestionStats } from "../models/question/QuestionStats";
import { QuestionFull } from "../models/question/QuestionFull";

export const useQuestionStore = defineStore("question", () => {
  const isLoading = ref(false);
  const activeQuestions: Ref<QuestionList[]> = ref([]);
  const closedQuestions: Ref<QuestionList[]> = ref([]);

  async function getAllActiveQuestions(): Promise<void> {
    try {
      isLoading.value = true;
      activeQuestions.value = await QuestionApi.getAllActiveQuestions();
    } catch (error) {
      throw error;
    } finally {
      isLoading.value = false;
    }
  }

  async function getAllClosedQuestions(): Promise<void> {
    try {
      isLoading.value = true;
      closedQuestions.value = await QuestionApi.getAllClosedQuestions();
    } catch (error) {
      throw error;
    } finally {
      isLoading.value = false;
    }
  }

  async function getQuestionStats(id: string): Promise<QuestionStats> {
    try {
      isLoading.value = true;
      return await QuestionApi.getQuestionStats(id);
    } catch (error) {
      throw error;
    } finally {
      isLoading.value = false;
    }
  }

  async function create(question: QuestionFull): Promise<string> {
    try {
      isLoading.value = true;
      return await QuestionApi.create(question);
    } catch (error) {
      throw error;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    isLoading,
    activeQuestions,
    closedQuestions,
    getAllActiveQuestions,
    getAllClosedQuestions,
    getQuestionStats,
    create,
  };
});
