import { MerchandiseUnitType } from '~/types';

const map: Record<MerchandiseUnitType, string> & Record<string, string> = {
  Kilograms: 'г',
  Liters: 'л',
  Milliliters: 'мл',
  Pieces: 'шт',
};

type GetUnitText = (unit: string) => string;

const getUnitText: GetUnitText = (unit) => map[unit] ?? '';

export { getUnitText };
