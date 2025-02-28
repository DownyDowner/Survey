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
  </v-row>
  <v-row class="mt-4">
    <v-col cols="12">
      <ag-charts :options="chartOptions" style="height: 400px" />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, Ref } from "vue";
import { useQuestionStore } from "../../stores/question";
import router from "../../router";
import { QuestionStats } from "../../models/question/QuestionStats";
import { NavigationConst } from "../../router/NavigationConst";
import { AgCharts } from "ag-charts-vue3";
import type { AgChartOptions } from "ag-charts-community";
import { useNotificationStore } from "../../stores/notification";

const questionStore = useQuestionStore();
const notificationStore = useNotificationStore();
const question: Ref<QuestionStats> = ref(new QuestionStats(null));
const chartOptions = ref<AgChartOptions | undefined>(undefined);

onMounted(async () => {
  await loadQuestionStats();
});

async function loadQuestionStats() {
  try {
    const id = router.currentRoute.value.params.id as string;
    question.value = await questionStore.getQuestionStats(id);
    chartOptions.value = {
      title: {
        text: "Vote Distribution",
        fontSize: 18,
      },
      data: question.value.choices.map((choice) => ({
        name: choice.name,
        voteCount: choice.voteCount,
      })),
      series: [{ type: "pie", angleKey: "voteCount", calloutLabelKey: "name" }],
    };
  } catch (error) {
    notificationStore.showError(
      "An error occurred while loading the question stats."
    );
    router.push({ name: NavigationConst.nameClosed });
  }
}

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
</script>
