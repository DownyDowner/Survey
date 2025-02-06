<template>
  <v-container>
    <QuestionsList
      ref="questionList"
      :questions="questionStore.activeQuestions"
    />
  </v-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useQuestionStore } from "../../stores/question";
import QuestionsList from "./components/QuestionsList.vue";

const questionStore = useQuestionStore();

const questionList = ref<InstanceType<typeof QuestionsList> | null>(null);

onMounted(async () => {
  await loadActiveQuestions();
});

async function loadActiveQuestions() {
  await questionStore.getAllActiveQuestions();
}
</script>
