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

const questionStore = useQuestionStore();

const questionList = ref<InstanceType<typeof QuestionsList> | null>(null);

onMounted(async () => {
  await loadClosedQuestions();
  questionList.value?.$forceUpdate;
});

async function loadClosedQuestions() {
  await questionStore.getAllClosedQuestions();
}
</script>
