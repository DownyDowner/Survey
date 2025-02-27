<template>
  <v-list v-if="props.questions.length > 0">
    <v-list-item
      v-for="question in props.questions"
      :key="question.id"
      :title="question.name"
      :subtitle="formatDateRange(question.beginDate, question.endDate)"
    >
      <template v-slot:append>
        <v-btn
          color="primary"
          icon="mdi-chevron-right"
          variant="text"
          @click.stop="openDetailsQuestion(question)"
        />
      </template>
    </v-list-item>
  </v-list>
  <v-alert v-else type="info" variant="outlined">
    No questions available at this time.
  </v-alert>
</template>

<script setup lang="ts">
import { QuestionList } from "../../../models/question/QuestionList";
import router from "../../../router";
import { NavigationConst } from "../../../router/NavigationConst";

const props = defineProps<{
  questions: QuestionList[];
  isClosed: boolean;
}>();

function formatDateRange(beginDate: string, endDate: string): string {
  const begin = new Date(beginDate).toLocaleDateString();
  const end = new Date(endDate).toLocaleDateString();

  return `From ${begin} to ${end}`;
}

function openDetailsQuestion(question: QuestionList) {
  if (props.isClosed) {
    router.push({
      name: NavigationConst.nameClosedDetails,
      params: { id: question.id },
    });
  }
}
</script>
