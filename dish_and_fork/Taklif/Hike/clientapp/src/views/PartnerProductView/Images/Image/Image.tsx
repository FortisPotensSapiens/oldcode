import { CrownOutlined, DeleteOutlined } from '@ant-design/icons';
import { ErrorOutline } from '@mui/icons-material';
import { Box, CircularProgress } from '@mui/material';
import { Button, Tooltip } from 'antd';
import { FC, MouseEventHandler, useRef } from 'react';
import { useDrag, useDrop, XYCoord } from 'react-dnd';

import { useGetFileById } from '~/api';
import { FileReadModel } from '~/types';

import { ImageValue, ItemTypes } from '../types';

type ImagesProps = {
  onClick: MouseEventHandler<HTMLDivElement>;
  value: ImageValue;
  selected?: boolean;
  showCrow?: boolean;
  index: number;
  isDraggingEnabled?: boolean;
  onSuccess?: (file: FileReadModel) => void;
  onDelete?: (index: number) => void | undefined | null;
  moveCard: (dragIndex: number, hoverIndex: number) => void;
};

const Image: FC<ImagesProps> = ({
  index,
  onClick,
  onDelete,
  onSuccess,
  selected,
  value,
  moveCard,
  isDraggingEnabled = true,
  showCrow = true,
}) => {
  const enabled = typeof value === 'string';
  const [id, initialData] = enabled ? [value, undefined] : [value.id, value];
  const { data, isError } = useGetFileById(id, { enabled, initialData, onSuccess });
  const path = data?.path ?? undefined;

  const ref = useRef<HTMLDivElement>(null);
  const [{ handlerId }, drop] = useDrop<any, void, { handlerId: any }>({
    accept: ItemTypes.IMAGE,
    collect(monitor) {
      return {
        handlerId: monitor.getHandlerId(),
      };
    },
    hover(item: any, monitor) {
      if (!ref.current) {
        return;
      }
      const dragIndex = item.index;
      const hoverIndex = index;

      if (dragIndex === hoverIndex) {
        return;
      }

      const hoverBoundingRect = ref.current?.getBoundingClientRect();
      const hoverMiddleY = (hoverBoundingRect.bottom - hoverBoundingRect.top) / 2;
      const clientOffset = monitor.getClientOffset();
      const hoverClientY = (clientOffset as XYCoord).y - hoverBoundingRect.top;

      if (dragIndex < hoverIndex && hoverClientY < hoverMiddleY) {
        return;
      }

      if (dragIndex > hoverIndex && hoverClientY > hoverMiddleY) {
        return;
      }

      moveCard(dragIndex, hoverIndex);

      item.index = hoverIndex;
    },
  });
  const [{ isDragging }, drag] = useDrag({
    type: ItemTypes.IMAGE,
    item: () => {
      return { id, index, image: path };
    },
    collect: (monitor: any) => ({
      isDragging: monitor.isDragging(),
    }),
  });

  const opacity = isDragging ? 0 : 1;

  const handleDelete = () => {
    onDelete?.(index);
  };

  if (isDraggingEnabled) {
    drag(drop(ref));
  }

  return (
    <Box
      borderRadius={2.5}
      data-index={index}
      height={100}
      onClick={onClick}
      overflow="hidden"
      position="relative"
      ref={ref}
      data-handler-id={handlerId}
      style={{
        opacity,
        overflow: 'hidden',
      }}
      width={100}
    >
      {!index && showCrow ? (
        <Tooltip title="Основное изображение, которое будет размещено на карточке товара">
          <CrownOutlined
            style={{
              position: 'absolute',
              background: 'rgba(255, 255, 255, 0.9)',
              padding: '5px',
              borderRadius: '10px',
              right: '5px',
              top: '5px',
            }}
          />
        </Tooltip>
      ) : undefined}
      {onDelete ? (
        <Button
          style={{
            marginLeft: '5px',
            padding: '3px 5px',
            position: 'absolute',
          }}
          onClick={handleDelete}
        >
          <DeleteOutlined />
        </Button>
      ) : undefined}
      <img
        style={{
          borderRadius: '10px',
          padding: '5px',
          border: '3px solid #7E004A',
          borderColor: !selected || !showCrow ? 'transparent' : '#7E004A',
          objectFit: 'contain',
        }}
        src={path}
        alt=""
        width="100px"
        height="100px"
      />
      {isError ? <ErrorOutline fontSize="large" /> : !data ? <CircularProgress color="inherit" size="large" /> : null}
    </Box>
  );
};

export { Image };
export type { ImagesProps };
