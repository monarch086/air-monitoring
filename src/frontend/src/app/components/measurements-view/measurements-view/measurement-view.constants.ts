import { PeriodSelectorItem } from 'src/app/model/period-selector-item';

export const PERIOD_ITEMS: PeriodSelectorItem[] = [
  { name: '1 day', value: 1 } as PeriodSelectorItem,
  { name: '3 days', value: 3 } as PeriodSelectorItem,
  { name: '5 days', value: 5 } as PeriodSelectorItem,
  { name: '1 month', value: 30 } as PeriodSelectorItem,
  { name: '3 months', value: 90 } as PeriodSelectorItem,
  { name: '6 months', value: 180 } as PeriodSelectorItem,
  { name: '1 year', value: 365 } as PeriodSelectorItem
];
