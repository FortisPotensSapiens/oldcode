import { MerchandiseUnitType } from '~/types';

type GetSmallerUnit = (unit: string) => string;

const map: Partial<Record<MerchandiseUnitType, string>> & Record<string, string> = {
  Kilograms: 'Grams',
  Liters: 'Milliliters',
};

const getSmallerUnit: GetSmallerUnit = (unit) => map[unit];

export { getSmallerUnit };
