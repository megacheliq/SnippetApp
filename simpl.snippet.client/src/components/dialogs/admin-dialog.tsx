import {
    AlertDialog,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import {
    Accordion,
    AccordionContent,
    AccordionItem,
    AccordionTrigger,
} from "@/components/ui/accordion"
import { ScrollArea } from "@/components/ui/scroll-area"
import { getAllSnippets, getRandomSnippet, getSnippetById } from "@/services/snippetService";
import { IAllSnippetsResponse, Level } from "@/abstract/snippetTypes";
import { Snippet } from "@/models/Snippet";
import { useEffect, useState } from "react";
import { LoadingSpinner } from "@/components/ui/loading-spinner";
import Editor from '@monaco-editor/react';
import { useTheme } from "@/components/theme-provider"
import { Button } from "@/components/ui/button";
import { useSnippetContext } from "@/contexts/SnippetContext";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"

const AdminDialog: React.FC = () => {
    const { theme } = useTheme();
    const [snippets, setSnippets] = useState<IAllSnippetsResponse[] | null>(null);
    const [snippetDetails, setSnippetDetails] = useState<{ [key: string]: Snippet }>({});
    const [loadingSnippet, setLoadingSnippet] = useState<string | null>(null);
    const [randomSnippetDetails, setRandomSnippetDetails] = useState<Snippet>();
    const [loadingRandomSnippet, setLoadingRandomSnippet] = useState(false);
    const [open, setOpen] = useState(false);
    const { setMainSnippet } = useSnippetContext();

    const levelValues = Object.values(Level).filter(value => !isNaN(Number(value))) as Level[];

    const levelOptions = levelValues.map(value => ({
        value: value,
        label: Level[value]
    }));

    const loadExactSnippet = (snippet: Snippet) => {
        setMainSnippet(snippet);
        setOpen(false);
    }

    const fetchSnippets = async () => {
        const data = await getAllSnippets();
        if (data) {
            setSnippets(data);
        }
    };

    const fetchRandomSnippet = async (level: string) => {
        setLoadingRandomSnippet(true);
        const data = await getRandomSnippet(level);
        if (data) {
            const snippet = new Snippet(
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
            );
            setRandomSnippetDetails(snippet);
        }
        setLoadingRandomSnippet(false);
    }

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

    useEffect(() => {
        fetchSnippets();
    }, []);

    return (
        <>
            <AlertDialog open={open} onOpenChange={setOpen}>
                <AlertDialogTrigger className='bg-primary text-primary-foreground rounded-sm px-3 font-semibold py-1'>
                    Выбрать сниппет
                </AlertDialogTrigger>
                <AlertDialogContent className="max-w-[75%]">
                    <AlertDialogHeader>
                        <AlertDialogDescription>
                            <Tabs defaultValue="all">
                                <TabsList className="w-full">
                                    <TabsTrigger className="w-1/2" value="all">Все сниппеты</TabsTrigger>
                                    <TabsTrigger className="w-1/2" value="rndm">Случайный</TabsTrigger>
                                </TabsList>
                                <TabsContent value="all">
                                    <Accordion type="single" collapsible>
                                        <ScrollArea className="h-[550px]">
                                            {snippets === null ? (
                                                <LoadingSpinner />
                                            ) : (
                                                snippets.map(snippet => (
                                                    <AccordionItem key={snippet.id} value={snippet.id} className="pr-4">
                                                        <AccordionTrigger onClick={() => fetchSnippet(snippet.id)}>
                                                            {snippet.theme}
                                                        </AccordionTrigger>
                                                        <AccordionContent className="max-w-[99%]">
                                                            {loadingSnippet === snippet.id ? (
                                                                <LoadingSpinner />
                                                            ) : (
                                                                snippetDetails[snippet.id] && (
                                                                    <div>
                                                                        <p><span className="text-accent-foreground">Автор:</span> {snippetDetails[snippet.id].authorName}</p>
                                                                        <p><span className="text-accent-foreground">Направление:</span> {snippetDetails[snippet.id].getDirectionLabel()}</p>
                                                                        <p><span className="text-accent-foreground">Уровень:</span> {snippetDetails[snippet.id].getLevelLabel()}</p>
                                                                        <p><span className="text-accent-foreground">Главный вопрос:</span> {snippetDetails[snippet.id].mainQuestion}</p>
                                                                        <p><span className="text-accent-foreground">Ответ:</span> {snippetDetails[snippet.id].solution}</p>
                                                                        <div>
                                                                            <p className="text-accent-foreground">Дополнительные вопросы:</p>
                                                                            {snippetDetails[snippet.id].additionalQuestions.map(additional => (
                                                                                <p>{additional}</p>
                                                                            ))}
                                                                        </div>
                                                                        <Editor
                                                                            className='pr-4 mt-2'
                                                                            height='20vh'
                                                                            language='csharp'
                                                                            defaultValue=""
                                                                            theme={theme === 'dark' ? 'vs-dark' : 'vs-light'}
                                                                            value={snippetDetails[snippet.id].codeSnippet}
                                                                            options={{
                                                                                wordWrap: 'on',
                                                                                minimap: {
                                                                                    enabled: false
                                                                                },
                                                                                fontSize: 10,
                                                                                readOnly: true
                                                                            }}
                                                                            loading={<LoadingSpinner />}
                                                                        />
                                                                        <div className="mt-2">
                                                                            <Button onClick={() => loadExactSnippet(snippetDetails[snippet.id])}>Выбрать</Button>
                                                                        </div>
                                                                    </div>
                                                                )
                                                            )}
                                                        </AccordionContent>
                                                    </AccordionItem>
                                                ))
                                            )}
                                        </ScrollArea>
                                    </Accordion>
                                </TabsContent>
                                <TabsContent value="rndm">
                                    <Accordion type="single" collapsible>
                                        <ScrollArea className="h-[400px]">
                                            {
                                                levelOptions.map(level => (
                                                    <AccordionItem key={level.value} value={level.label} className="pr-4">
                                                        <AccordionTrigger onClick={() => fetchRandomSnippet(level.label)}>
                                                            {level.label}
                                                        </AccordionTrigger>
                                                        <AccordionContent className="max-w-[99%]">
                                                            {loadingRandomSnippet ? (
                                                                <LoadingSpinner />
                                                            ) : (
                                                                randomSnippetDetails && (
                                                                    <div>
                                                                        <p><span className="text-accent-foreground">Автор:</span> {randomSnippetDetails.authorName}</p>
                                                                        <p><span className="text-accent-foreground">Направление:</span> {randomSnippetDetails.getDirectionLabel()}</p>
                                                                        <p><span className="text-accent-foreground">Уровень:</span> {randomSnippetDetails.getLevelLabel()}</p>
                                                                        <p><span className="text-accent-foreground">Главный вопрос:</span> {randomSnippetDetails.mainQuestion}</p>
                                                                        <p><span className="text-accent-foreground">Ответ:</span> {randomSnippetDetails.solution}</p>
                                                                        <div>
                                                                            <p className="text-accent-foreground">Дополнительные вопросы:</p>
                                                                            {randomSnippetDetails.additionalQuestions.map(additional => (
                                                                                <p>{additional}</p>
                                                                            ))}
                                                                        </div>
                                                                        <Editor
                                                                            className='pr-4 mt-2'
                                                                            height='20vh'
                                                                            language='csharp'
                                                                            defaultValue=""
                                                                            theme={theme === 'dark' ? 'vs-dark' : 'vs-light'}
                                                                            value={randomSnippetDetails.codeSnippet}
                                                                            options={{
                                                                                wordWrap: 'on',
                                                                                minimap: {
                                                                                    enabled: false
                                                                                },
                                                                                fontSize: 10,
                                                                                readOnly: true
                                                                            }}
                                                                            loading={<LoadingSpinner />}
                                                                        />
                                                                        <div className="mt-2">
                                                                            <Button onClick={() => loadExactSnippet(randomSnippetDetails)}>Выбрать</Button>
                                                                        </div>
                                                                    </div>
                                                                )
                                                            )}
                                                        </AccordionContent>
                                                    </AccordionItem>
                                                ))
                                            }
                                        </ScrollArea>
                                    </Accordion>
                                </TabsContent>
                            </Tabs>
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                        <AlertDialogCancel>Назад</AlertDialogCancel>
                    </AlertDialogFooter>
                </AlertDialogContent>
            </AlertDialog>
        </>
    );
}

export default AdminDialog;
