import React, { useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import { createSession } from '@/services/shareService';
import { Button } from '@/components/ui/button';
import { UserNav } from '@/components/user-nav';
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import { DataTable } from '@/components/tables/adminSnippet/data-table';
import { deleteSnippetById, getAllSnippets, getSnippetById } from "@/services/snippetService";
import { IAllSnippetsResponse } from '@/abstract/snippetTypes';
import { createColumns } from '@/components/tables/adminSnippet/columns';
import { Snippet } from "@/models/Snippet";
import { useDialogContext } from "@/contexts/DialogContext";
import ReadDialog from '../dialogs/read-dialog';
import EditDialog from '../dialogs/edit-dialog';
import DeleteDialog from '../dialogs/delete-dialog';
import AddDialog from '../dialogs/add-dialog';
import AddFileDialog from '../dialogs/add-file-dialog';
import { useStateContext } from '@/contexts/ContextProvider';

const Hub: React.FC = () => {
    const [snippets, setSnippets] = useState<IAllSnippetsResponse[] | []>([]);
    const [userSnippets, setUserSnippets] = useState<IAllSnippetsResponse[] | []>([]);
    const [selectedSnippet, setSelectedSnippet] = useState<IAllSnippetsResponse | null>(null);
    const {
        setIsEditAlertDialogOpen,
        setIsReadAlertDialogOpen,
        setIsDeleteAlertDialogOpen,
    } = useDialogContext();
    const [snippetDetails, setSnippetDetails] = useState<{ [key: string]: Snippet }>({});
    const [loadingSnippet, setLoadingSnippet] = useState<string | null>(null);
    const navigate = useNavigate();
    const { role, user } = useStateContext();

    const templateContent = `// Тема: Асинхронное программирование\n// Уровень: Junior\n// Основной вопрос: Что будет выведено на консоль при выполнении данного кода? Почему?\n// Ответ: Будет выведено\n// Starting...\n// Task1 started\n// Task2 started\n// Task2 finished\n// Task1 finished\n// Finished!\n// Это происходит потому, что запускаются две асинхронные задачи Task1 и Task2 параллельно с помощью операторов await.\n// Task2 завершается первым, так как имеет меньшую задержку (1 секунда по сравнению с 2 секундами у Task1).\n// После завершения обеих задач, программа выводит "Finished!"\n// Дополнительные вопросы:\n// Что такое модификатор async в сигнатуре метода и зачем он используется?\n// Какой будет результат, если изменить вызов Task.WhenAll(task1, task2) на await Task.WhenAny(task1, task2)?\n// Как можно изменить код, чтобы задача Task2 всегда завершалась после задачи Task1, независимо от времени задержки?\nusing System;\nusing System.Threading.Tasks;\nConsole.WriteLine("Starting...");\nvar task1 = Task1();\nvar task2 = Task2();\nawait Task.WhenAll(task1, task2);\nConsole.WriteLine("Finished!");\nasync Task Task1()\n{\nConsole.WriteLine("Task1 started");\nawait Task.Delay(2000);\nConsole.WriteLine("Task1 finished");\n}\nasync Task Task2()\n{\nConsole.WriteLine("Task2 started");\nawait Task.Delay(1000);\nConsole.WriteLine("Task2 finished");\n}`;

    const downloadTemplate = () => {
        const element = document.createElement("a");
        const file = new Blob([templateContent], { type: 'text/plain' });
        element.href = URL.createObjectURL(file);
        element.download = "template.cs";
        document.body.appendChild(element);
        element.click();
        document.body.removeChild(element);
    };

    const columns = createColumns({
        onView: (snippet) => {
            setSelectedSnippet(snippet);
            setIsReadAlertDialogOpen(true);
        },
        onEdit: (snippet) => {
            setSelectedSnippet(snippet);
            setIsEditAlertDialogOpen(true);
        },
        onDelete: (snippet) => {
            setSelectedSnippet(snippet);
            setIsDeleteAlertDialogOpen(true);
        }
    });

    const fetchSnippets = async () => {
        const data = await getAllSnippets();
        if (data) {
            setSnippets(data);
        }
    };

    const handleCreateSession = async () => {
        const response = await createSession();
        navigate(`/code/${response.sessionId}`);
    };

    const fetchSnippet = async (id: string) => {
        setLoadingSnippet(id);
        const data = await getSnippetById(id);
        if (data) {
            setSnippetDetails(prevDetails => ({
                ...prevDetails,
                [id]: new Snippet(
                    data.id,
                    data.theme,
                    data.authorId,
                    data.authorName,
                    data.direction,
                    data.level,
                    data.codeSnippet,
                    data.mainQuestion,
                    data.solution,
                    data.additionalQuestions
                )
            }));
        }
        setLoadingSnippet(null);
    };

    const formatUserSnippets = (id: string) => {
        const formattedUserSnippets = snippets
            .filter(snippet => snippet.authorId === id)
            .map(snippet => ({
                id: snippet.id,
                authorId: snippet.authorId,
                authorName: snippet.authorName,
                theme: snippet.theme,
                createdDate: snippet.createdDate,
                modifiedDate: snippet.modifiedDate
            }));
        setUserSnippets(formattedUserSnippets);
    }

    const deleteSnippet = async (id: string) => {
        deleteSnippetById(id);
        fetchSnippets();
    }

    useEffect(() => {
        if (role === "user") {
            formatUserSnippets(user?.user_id || "");
        }
    }, [snippets])

    useEffect(() => {
        fetchSnippets();
    }, []);

    useEffect(() => {
        if (selectedSnippet) {
            fetchSnippet(selectedSnippet.id);
        }
    }, [selectedSnippet]);

    return (
        <>
            <div className='h-screen'>
                <div className="border-b">
                    <div className="flex h-12 items-center px-4">
                        <div className="ml-auto flex items-center space-x-4">
                            {role === "admin" && (
                                <Button onClick={handleCreateSession} >
                                    Создать сессию
                                </Button>
                            )}
                            <UserNav />
                        </div>
                    </div>
                </div>
                <div className="flex justify-center p-8">
                    {role === "admin" ? (
                        <Card className='w-full h-auto'>
                            <CardHeader>
                                <CardTitle>
                                    Коллекция всех сниппетов
                                </CardTitle>
                                <CardDescription>
                                    Здесь можно изменять все сниппеты
                                </CardDescription>
                            </CardHeader>
                            <CardContent>
                                <DataTable data={snippets} columns={columns} />
                            </CardContent>
                        </Card>
                    ) : (
                        <Card className='w-full h-auto'>
                            <CardHeader>
                                <CardTitle>
                                    Коллекция ваших сниппетов
                                </CardTitle>
                                <CardDescription>
                                    Здесь можно изменять ваши сниппеты
                                </CardDescription>
                            </CardHeader>
                            <CardContent>
                                <DataTable data={userSnippets} columns={columns} />
                            </CardContent>
                        </Card>
                    )}

                </div>

            </div>

            <ReadDialog
                selectedSnippet={selectedSnippet}
                loadingSnippet={loadingSnippet}
                snippetDetails={snippetDetails}
            />

            <EditDialog
                selectedSnippet={selectedSnippet}
                loadingSnippet={loadingSnippet}
                snippetDetails={snippetDetails}
                fetchSnippets={fetchSnippets}
            />

            <DeleteDialog
                selectedSnippet={selectedSnippet}
                deleteSnippet={deleteSnippet}
            />

            <AddDialog fetchSnippets={fetchSnippets} />

            <AddFileDialog fetchSnippets={fetchSnippets} downloadTemplate={downloadTemplate} />
        </>
    );
};

export default Hub;