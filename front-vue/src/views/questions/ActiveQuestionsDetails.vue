<template>
  <div>Test</div>
</template>

<script setup lang="ts">
import router from "../../router";
import { onMounted, Ref, ref } from "vue";
import { QuestionFull } from "../../models/question/QuestionFull";
import { useQuestionStore } from "../../stores/question";
import { NavigationConst } from "../../router/NavigationConst";
import { useNotificationStore } from "../../stores/notification";

const questionStore = useQuestionStore();
const notificationStore = useNotificationStore();

const question: Ref<QuestionFull> = ref(new QuestionFull(null));

onMounted(async () => {
  await loadQuestion();
});

async function loadQuestion() {
  try {
    const id = router.currentRoute.value.params.id as string;
    question.value = await questionStore.getQuestionWithUserVotes(id);
    console.log("Question", question.value);
  } catch (error) {
    notificationStore.showError(
      "An error occurred while loading the question."
    );
    router.push({ name: NavigationConst.nameHome });
  }
}
</script>
