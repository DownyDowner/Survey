<template>
  <v-container>
    <QuestionsList
      ref="questionList"
      :questions="questionStore.closedQuestions"
      :is-closed="true"
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
  await loadClosedQuestions();
});

async function loadClosedQuestions() {
  try {
    await questionStore.getAllClosedQuestions();
  } catch (error) {
    notificationStore.showError(
      "An error occurred while loading the closed questions."
    );
  }
}
</script>
