<template>
  <v-list>
    <v-list-item
      v-for="question in questionStore.activeQuestions"
      :key="question.id"
      :title="question.name"
    />
  </v-list>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useQuestionStore } from "../../stores/question";

const questionStore = useQuestionStore();

onMounted(async () => {
  await loadActiveQuestions();
});

async function loadActiveQuestions() {
  await questionStore.getAllActiveQuestions();
}
</script>
