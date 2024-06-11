import { IAllSnippetsResponse } from "@/abstract/snippetTypes"
import { ColumnDef } from "@tanstack/react-table"
import { Button } from "@/components/ui/button"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { ArrowUpDown, MoreHorizontal } from "lucide-react"

interface ColumnsProps {
  onView: (snippet: IAllSnippetsResponse) => void;
  onEdit: (snippet: IAllSnippetsResponse) => void;
  onDelete: (snippet: IAllSnippetsResponse) => void;
}

export const createColumns = ({ onView, onEdit, onDelete }: ColumnsProps): ColumnDef<IAllSnippetsResponse>[] => [
  {
    accessorKey: "authorName",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Автор
          <ArrowUpDown className="ml-2 h-4 w-4" />
        </Button>
      )
    },
  },
  {
    accessorKey: "theme",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Тема
          <ArrowUpDown className="ml-2 h-4 w-4" />
        </Button>
      )
    },
  },
  {
    accessorKey: "createdDate",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Дата создания
          <ArrowUpDown className="ml-2 h-4 w-4" />
        </Button>
      )
    },
    cell: ({ row }) => {
      const dateStr = row.getValue('createdDate');
      if (typeof dateStr === 'string') {
        const date = new Date(dateStr);

        const padZero = (num: number) => num.toString().padStart(2, '0');

        const day = padZero(date.getDate());
        const month = padZero(date.getMonth() + 1);
        const year = date.getFullYear();
        const hours = padZero(date.getHours());
        const minutes = padZero(date.getMinutes());
        const seconds = padZero(date.getSeconds());

        return `${day}.${month}.${year} ${hours}:${minutes}:${seconds}`;
      } else {
        console.error('Invalid date string:', dateStr);
        return '';
      }

    }
  },
  {
    accessorKey: "modifiedDate",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Дата изменения
          <ArrowUpDown className="ml-2 h-4 w-4" />
        </Button>
      )
    },
    cell: ({ row }) => {
      const dateStr = row.getValue('modifiedDate');
      if (typeof dateStr === 'string') {
        const date = new Date(dateStr);

        const padZero = (num: number) => num.toString().padStart(2, '0');

        const day = padZero(date.getDate());
        const month = padZero(date.getMonth() + 1);
        const year = date.getFullYear();
        const hours = padZero(date.getHours());
        const minutes = padZero(date.getMinutes());
        const seconds = padZero(date.getSeconds());

        return `${day}.${month}.${year} ${hours}:${minutes}:${seconds}`;
      } else {
        console.error('Invalid date string:', dateStr);
        return '';
      }

    }
  },
  {
    id: "actions",
    cell: ({ row }) => {
      const snippet = row.original

      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" className="h-8 w-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Действия</DropdownMenuLabel>
            <DropdownMenuItem className="cursor-pointer" onClick={() => onView(snippet)}>Просмотреть</DropdownMenuItem>
            <DropdownMenuItem className="cursor-pointer" onClick={() => onEdit(snippet)}>Редактировать</DropdownMenuItem>
            <DropdownMenuItem className="cursor-pointer" onClick={() => onDelete(snippet)}>Удалить</DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      )
    },
  },
]
