import { OrderItemReadModel } from '~/types';

const getSummServingSize = (items: OrderItemReadModel[] | null | undefined) => {
  let summ = 0;

  items?.forEach((item) => {
    summ += item.servingGrossWeightInKilograms * (item.amount ?? 1);
  });

  return summ;
};

export { getSummServingSize };
