<template>
  <v-container>
    <QuestionsList
      ref="questionList"
      :questions="questionStore.activeQuestions"
      :is-closed="false"
      @on-question-deleted="loadActiveQuestions"
    />
  </v-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useQuestionStore } from "../../stores/question";
import QuestionsList from "./components/QuestionsList.vue";
import { useNotificationStore } from "../../stores/notification";

const questionStore = useQuestionStore();
const notificationStore = useNotificationStore();

const questionList = ref<InstanceType<typeof QuestionsList> | null>(null);

onMounted(async () => {
  await loadActiveQuestions();
});

async function loadActiveQuestions() {
  try {
    await questionStore.getAllActiveQuestions();
  } catch (error) {
    notificationStore.showError(
      "An error occurred while loading the active questions."
    );
  }
}
</script>
