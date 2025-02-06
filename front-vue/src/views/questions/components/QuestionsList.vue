<template>
  <v-list>
    <v-list-item
      v-for="question in props.questions"
      :key="question.id"
      :title="question.name"
      :subtitle="formatDateRange(question.beginDate, question.endDate)"
    >
      <template v-slot:append>
        <v-btn color="primary" icon="mdi-chevron-right" variant="text" />
      </template>
    </v-list-item>
  </v-list>
</template>

<script setup lang="ts">
import { QuestionList } from "../../../models/question/QuestionList";

const props = defineProps<{
  questions: QuestionList[];
}>();

function formatDateRange(beginDate: string, endDate: string): string {
  const begin = new Date(beginDate).toLocaleDateString();
  const end = new Date(endDate).toLocaleDateString();

  return `From ${begin} to ${end}`;
}
</script>
