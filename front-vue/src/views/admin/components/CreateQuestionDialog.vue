<template>
  <v-dialog
    v-model="isOpen"
    no-click-animation
    scroll-strategy="close"
    max-width="600"
    max-height="600"
    persistent
  >
    <v-form v-model="isValid" @submit.prevent>
      <v-card min-height="200">
        <v-toolbar flat dark color="primary">
          <v-toolbar-title>Create a Question</v-toolbar-title>
          <v-spacer />
          <v-btn icon dark @click.stop="close">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text>
          <v-row no-gutters>
            <v-col cols="12" md="6" class="mb-2">
              <v-text-field
                label="Name"
                rounded="lg"
                :rules="questionNameRules"
                v-model="questionName"
              />
            </v-col>
            <v-col cols="12" md="6" class="mb-2">
              <v-checkbox label="Multiple choices" v-model="isMultiple" />
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field
                label="Begin date"
                rounded="lg"
                type="date"
                :rules="dateRules.beginDate"
                v-model="beginDate"
              />
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field
                class="mb-2"
                label="Ending date"
                rounded="lg"
                type="date"
                :rules="dateRules.endDate"
                v-model="endDate"
              />
            </v-col>
            <v-col cols="12" class="d-flex flex-column align-end">
              <v-tooltip location="top">
                <template v-slot:activator="{ props }">
                  <v-btn
                    v-bind="props"
                    density="compact"
                    icon="mdi-plus"
                    class="align-self-end"
                    color="warning"
                    @click.stop="openCreateQuestionChoiceDialog"
                  />
                </template>
                <span>Add Choice</span>
              </v-tooltip>
            </v-col>
            <v-col cols="12">
              <v-list>
                <v-list-item
                  v-for="(choice, index) in choices"
                  :key="choice.name"
                  :title="choice.name"
                >
                  <template v-slot:append>
                    <v-btn
                      color="error"
                      icon="mdi-delete"
                      density="compact"
                      @click.stop="removeChoice(index)"
                    />
                  </template>
                </v-list-item>
              </v-list>
              <v-alert
                v-if="choicesError"
                type="error"
                variant="outlined"
                class="mt-2"
              >
                You must add at least two choices.
              </v-alert>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-btn
            class="ma-2 px-4"
            color="primary"
            variant="flat"
            size="large"
            @click.stop="close"
          >
            Cancel
          </v-btn>
          <v-btn
            type="submit"
            class="ma-2 px-4"
            color="success"
            variant="flat"
            size="large"
            :disabled="!isValid"
            @click.stop="saveQuestion"
            :loading="isLoading"
          >
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-form>
  </v-dialog>
  <CreateQuestionChoiceDialog
    ref="createQuestionChoiceDialog"
    @on-choice-added="onChoiceAdded"
  />
</template>

<script setup lang="ts">
import { ref, watchEffect } from "vue";
import CreateQuestionChoiceDialog from "./CreateQuestionChoiceDialog.vue";
import { useQuestionStore } from "../../../stores/question";
import { useNotificationStore } from "../../../stores/notification";
import { ChoiceSave } from "../../../models/question/ChoiceSave";
import { QuestionSave } from "../../../models/question/QuestionSave";

const questionStore = useQuestionStore();
const notificationStore = useNotificationStore();

const emit = defineEmits<{
  (e: "onQuestionAdded"): void;
}>();

const isOpen = ref(false);
const isValid = ref(false);
const isLoading = ref(false);
const createQuestionChoiceDialog = ref<InstanceType<
  typeof CreateQuestionChoiceDialog
> | null>(null);

const today = new Date().toISOString().split("T")[0];

const questionName = ref("");
const isMultiple = ref(false);
const beginDate = ref(today);
const endDate = ref("");
const choices = ref<ChoiceSave[]>([]);

const questionNameRules = [(v: string) => !!v || "Question name is required"];
const isValidDate = (date: string): boolean => {
  return !isNaN(new Date(date).getTime());
};
const dateRules = {
  beginDate: [
    (value: string) => !!value || "Begin date is required",
    (value: string) => (isValidDate(value) ? true : "Invalid date format"),
    (value: string) => value >= today || "Begin date cannot be in the past",
  ],
  endDate: [
    (value: string) => !!value || "Ending date is required",
    (value: string) => (isValidDate(value) ? true : "Invalid date format"),
    (value: string) =>
      new Date(value) > new Date(beginDate.value) ||
      "Ending date must be after begin date",
  ],
};
const choicesError = ref(false);
watchEffect(() => {
  if (choices.value.length >= 2) {
    choicesError.value = false;
  }
});

const open = () => {
  isOpen.value = true;
};

const close = () => {
  isOpen.value = false;
  questionName.value = "";
  beginDate.value = today;
  endDate.value = "";
  isMultiple.value = false;
  choices.value = [];
};

function openCreateQuestionChoiceDialog() {
  createQuestionChoiceDialog.value?.open();
}

function onChoiceAdded(choice: ChoiceSave) {
  choices.value.push(choice);
}

function removeChoice(index: number) {
  choices.value.splice(index, 1);
}

async function saveQuestion() {
  try {
    isLoading.value = true;
    if (choices.value.length < 2) {
      choicesError.value = true;
    } else {
      const question = new QuestionSave({
        name: questionName.value,
        beginDate: new Date(beginDate.value).toISOString(),
        endDate: new Date(endDate.value).toISOString(),
        multiple: isMultiple.value,
        choices: choices.value.map((choice) => ({
          name: choice.name,
        })),
      });
      const questionId = await questionStore.create(question);
      notificationStore.showSuccess(
        `Question "${questionId}" has been successfully saved.`
      );
      emit("onQuestionAdded");
      close();
    }
  } catch (error) {
    notificationStore.showError(
      "Failed to save the question. Please try again."
    );
  } finally {
    isLoading.value = false;
  }
}

defineExpose({ open });
</script>
