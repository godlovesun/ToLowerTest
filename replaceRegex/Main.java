package regexReplace;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.Stack;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Main {

    public static void main(String[] args) {
        ArrayList<String> input = new ArrayList<String>();
        input.add("(?<!por10\\s+)(?<!por10\\s*)(cien(to)?))");
        input.add("(?<!por10\\s*)(cien(to)?))");
        input.add("(?<!%|\\d)");
        input.add("((((?<!(\\d+\\s*)|[-?])[-?])|[??])\\s*)?[\\d??????????]+");
        input.add("(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+(?!([,\\.]\\d+[a-zA-Z]))(?=\\D|\\b)");
        input.add("(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+(?!([\\.,]\\d+[a-zA-Z]))(?=\\b)");
        System.out.printf("number of sentences: %d\n", input.size());
        replace_3(input);
    }

    public static void replace_1(ArrayList<String> textlist) {
        Map<String, String> replace = new HashMap();
        replace.put("+", "{1, 10}");
        replace.put("*", "{0, 10}");
        Pattern pattern = Pattern.compile("(?<=(\\?<[!=][^)]{1,100}))(\\+|\\*)");
        for (String text : textlist) {
            Matcher matcher = pattern.matcher(text);
            if (matcher.find()) {
                text = matcher.replaceAll(replace.get(matcher.group(2)));
            }
            System.out.printf("after replace: %s\n", text);
        }
    }

    public static void replace_2(ArrayList<String> textlist) {
        Map<String, String> replace = new HashMap();
        replace.put("+", "{1, 10}");
        replace.put("*", "{0, 10}");
        Pattern pattern = Pattern.compile("(\\?<[!=][^)]+)(\\+|\\*)");
        for (String text : textlist) {
            Matcher matcher = pattern.matcher(text);
            if (matcher.find()) {
                text = matcher.replaceAll(matcher.group(1) + replace.get(matcher.group(2)));
            }
            System.out.printf("after replace: %s\n", text);
        }
    }


    public static void replace_3(ArrayList<String> textlist) {
        Map<Character, String> replace = new HashMap();
        replace.put('+', "{1, 10}");
        replace.put('*', "{0, 10}");

        Pattern lookBehindPattern = Pattern.compile("(\\?<[!=])");
        for (String text : textlist) {
            System.out.printf("Processing text: %s\n", text);
            Stack<Integer> toReplaceStack = new Stack<>();
            Matcher m = lookBehindPattern.matcher(text);
            // If use String.indexof() may be a little faster, case there exists recursive lookbehind group
            while (m.find()) {
                getReplaceIdx(text, m.start(), toReplaceStack);
            }

            System.out.printf("Found all index to replace: %s\n", toReplaceStack.toString());
            StringBuilder resText = new StringBuilder(text);
            while (!toReplaceStack.isEmpty()) {
                int idx = toReplaceStack.peek();
                resText.replace(idx, idx + 1, replace.get(text.charAt(idx)));
                toReplaceStack.pop();
            }
            String res = resText.toString();

            System.out.printf("Final string: %s\n\n", res);
        }
    }

    public static void getReplaceIdx(String oriText, int start, Stack<Integer> toReplace) {
        int idx = start + 3;
        Stack<Character> stack = new Stack<>();
        while (idx < oriText.length()) {
            switch (oriText.charAt(idx)) {
                case ')':
                    if (stack.isEmpty()) idx = oriText.length();
                    else stack.pop();
                    break;
                case '(':
                    stack.push('(');
                    break;
                case '*':
                case '+':
                    toReplace.push(idx);
                    break;
                case '|':
                    if (stack.isEmpty()) idx = oriText.length();
                    break;
            }
            idx += 1;
        }
    }
}
