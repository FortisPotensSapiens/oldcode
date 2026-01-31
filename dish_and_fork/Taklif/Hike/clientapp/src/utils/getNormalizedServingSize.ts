import { MerchandiseUnitType } from '~/types';

type GetNormalizedServingSize = (size: number, unit: string) => string;

const normalize: Record<MerchandiseUnitType | string, (unit: number) => number> = {
  Kilograms: (unit: number) => (unit < 1 ? unit * 1000 : unit),
  Liters: (unit: number) => unit,
  Pieces: (unit: number) => unit,
};

const normalizeSave: Record<MerchandiseUnitType | string, (unit: number) => number> = {
  Kilograms: (unit: number) => unit / 1000,
  Liters: (unit: number) => unit,
  Pieces: (unit: number) => unit,
};

const getNormalizedServingSizeNumber = (size: number | undefined, unit: string) => {
  if (!size) {
    return 0;
  }

  return normalize[unit](size);
};

const getNormalizedServingSize: GetNormalizedServingSize = (size, unit) => {
  const unitMapper: Record<MerchandiseUnitType | string, string> = {
    [MerchandiseUnitType.Kilograms]: size >= 1 ? 'kilogram' : 'gram',
    [MerchandiseUnitType.Pieces]: '',
    [MerchandiseUnitType.Liters]: 'liter',
  };

  const formatterOptions: Record<string, unknown> = {};

  if (unitMapper[unit]) {
    formatterOptions.style = 'unit';
    formatterOptions.unit = unitMapper[unit];
  }

  const formatter = new Intl.NumberFormat('ru-RU', formatterOptions);

  return `${formatter.format(getNormalizedServingSizeNumber(size, unit))} ${!unitMapper[unit] ? 'шт' : ''}`;
};

const getNormalizedSaveUnit = (value: number, unit: string) => {
  return normalizeSave[unit](value);
};

export { getNormalizedSaveUnit, getNormalizedServingSize, getNormalizedServingSizeNumber };
