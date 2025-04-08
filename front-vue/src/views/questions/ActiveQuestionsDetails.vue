<template>
  <v-row class="my-1">
    <v-col cols="12" md="6" class="d-flex">
      <v-text-field
        v-model="question.name"
        label="Name of question"
        hide-details
        readonly
      />
    </v-col>
    <v-col cols="12" sm="6" md="3" class="d-flex">
      <v-text-field
        v-model="formattedBeginDate"
        label="Begin Date"
        type="date"
        hide-details
        readonly
      />
    </v-col>
    <v-col cols="12" sm="6" md="3" class="d-flex">
      <v-text-field
        v-model="formattedEndDate"
        label="End Date"
        type="date"
        hide-details
        readonly
      />
    </v-col>
    <v-col cols="12">
      <v-container fluid class="d-flex align-center justify-center">
        <v-card title="Choices" rounded="lg">
          <v-card-text>
            <v-radio-group
              v-if="!question.multiple"
              v-model="selected"
              :readonly="!canVote"
            >
              <v-radio
                v-for="choice in question.choices"
                :key="choice.id"
                :label="choice.name"
                :value="choice.id"
              />
            </v-radio-group>

            <div v-else>
              <v-checkbox
                v-for="choice in question.choices"
                :key="choice.id"
                :label="choice.name"
                v-model="selected"
                :value="choice.id"
                :readonly="!canVote"
              />
            </div>
          </v-card-text>
          <v-card-actions v-if="canVote">
            <v-btn
              type="submit"
              class="ma-2 px-4"
              color="success"
              variant="flat"
              size="large"
              :disabled="selected.length <= 0"
              @click.stop="submit"
            >
              Submit
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-container>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import router from "../../router";
import { computed, onMounted, Ref, ref } from "vue";
import { QuestionFull } from "../../models/question/QuestionFull";
import { useQuestionStore } from "../../stores/question";
import { NavigationConst } from "../../router/NavigationConst";
import { useNotificationStore } from "../../stores/notification";

const questionStore = useQuestionStore();
const notificationStore = useNotificationStore();

const question: Ref<QuestionFull> = ref(new QuestionFull(null));
const canVote = ref(false);

const selected = ref<string | string[]>([]);

const formattedBeginDate = computed({
  get: () =>
    question.value.beginDate
      ? new Date(question.value.beginDate).toISOString().split("T")[0]
      : "",
  set: (val: string) => {
    question.value.beginDate = new Date(val).toISOString();
  },
});

const formattedEndDate = computed({
  get: () =>
    question.value.endDate
      ? new Date(question.value.endDate).toISOString().split("T")[0]
      : "",
  set: (val: string) => {
    question.value.endDate = new Date(val).toISOString();
  },
});

onMounted(async () => {
  await loadQuestion();

  if (!question.value.multiple) {
    selected.value = question.value.userVotes[0] || "";
  } else {
    selected.value = [...question.value.userVotes];
  }
});

async function loadQuestion() {
  try {
    const id = router.currentRoute.value.params.id as string;
    question.value = await questionStore.getQuestionWithUserVotes(id);
    canVote.value = question.value.userVotes.length === 0;
  } catch (error) {
    notificationStore.showError(
      "An error occurred while loading the question."
    );
    router.push({ name: NavigationConst.nameHome });
  }
}

async function submit() {
  try {
    const selectedList = Array.isArray(selected.value)
      ? selected.value
      : [selected.value];

    await questionStore.submit(question.value.id, selectedList);

    notificationStore.showSuccess(
      "Your answer has been submitted successfully!"
    );
    location.reload();
  } catch (error) {
    notificationStore.showError(
      "Oops! Something went wrong while submitting your answer. Please try again."
    );
  }
}
</script>
