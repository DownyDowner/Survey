import { defineStore } from "pinia";
import { Ref, ref } from "vue";
import { QuestionList } from "../models/question/QuestionList";
import { QuestionApi } from "../apis/QuestionApi";

export const useQuestionStore = defineStore("question", () => {
  const isLoading = ref(false);
  const activeQuestions: Ref<QuestionList[]> = ref([]);

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

  return {
    isLoading,
    activeQuestions,
    getAllActiveQuestions,
  };
});
