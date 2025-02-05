<template>
  <v-container>
    <v-list>
      <v-list-item
        v-for="question in questionStore.activeQuestions"
        :key="question.id"
        :title="question.name"
        :subtitle="formatDateRange(question.beginDate, question.endDate)"
      >
        <template v-slot:append>
          <v-btn color="primary" icon="mdi-chevron-right" variant="text" />
        </template>
      </v-list-item>
    </v-list>
  </v-container>
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

function formatDateRange(beginDate: string, endDate: string): string {
  const begin = new Date(beginDate).toLocaleDateString();
  const end = new Date(endDate).toLocaleDateString();

  return `From ${begin} to ${end}`;
}
</script>
